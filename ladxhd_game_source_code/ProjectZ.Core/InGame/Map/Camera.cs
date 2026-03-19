using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.Map
{
    public class Camera
    {
        public Vector2 Location;
        public Vector2 MoveLocation;
        private Rectangle fieldRect;

        public float Scale = 4;
        public float ShakeOffsetX;
        public float ShakeOffsetY;
        public float CameraFollowMultiplier = 1;

        public int ScaleValue => Math.Max(1, (int)MathF.Round(Scale));

        private int SnappedViewportWidth => (_viewportWidth / Math.Max(1, ScaleValue)) * Math.Max(1, ScaleValue);
        private int SnappedViewportHeight => (_viewportHeight / Math.Max(1, ScaleValue)) * Math.Max(1, ScaleValue);

        private float RoundedShakeX => MathF.Round(ShakeOffsetX);
        private float RoundedShakeY => MathF.Round(ShakeOffsetY);

        public int RoundX => (int)MathF.Round(Location.X + RoundedShakeX * Scale, MidpointRounding.AwayFromZero);
        public int RoundY => (int)MathF.Round(Location.Y + RoundedShakeY * Scale, MidpointRounding.AwayFromZero);

        public Matrix TransformMatrix
        {
            get
            {
                if (_viewportWidth <= 0 || _viewportHeight <= 0 || Scale <= 0f)
                    return Matrix.Identity;

                int scale = ScaleValue;
                int snappedWidth  = (_viewportWidth  / scale) * scale;
                int snappedHeight = (_viewportHeight / scale) * scale;
                int offsetX = (_viewportWidth  - snappedWidth)  / 2;
                int offsetY = (_viewportHeight - snappedHeight) / 2;
                int centerX = offsetX + snappedWidth  / 2;
                int centerY = offsetY + snappedHeight / 2;

                #if ANDROID
                // If height doesn't snap cleanly, nudge center by 1px to compensate for driver rounding
                if (_viewportHeight % scale != 0)
                    centerY += 1;
                #endif

                float tx = MathF.Round(centerX - RoundX);
                float ty = MathF.Round(centerY - RoundY);

                return
                    Matrix.CreateScale(scale, scale, 1f) *
                    Matrix.CreateTranslation(tx, ty, 0f);
            }
        }

        public static bool  SnapCamera;
        public static float SnapCameraTimer;

        public int X => (int)Math.Round(Location.X + ShakeOffsetX * Scale);
        public int Y => (int)Math.Round(Location.Y + ShakeOffsetY * Scale);

        private Vector2 _cameraDistance;

        private int _viewportWidth;
        private int _viewportHeight;

        public static bool ClassicMode => 
            (!GameSettings.ClassicCamera && 
            GameSettings.ModernOverworld && 
            (MapManager.ObjLink?.Map?.IsOverworld == false)) 
            || 
            (GameSettings.ClassicCamera && 
            (!GameSettings.ClassicDungeon || 
            MapManager.ObjLink?.Map?.DungeonMode == true || 
            MapManager.ObjLink?.Map?.DungeonEgg == true || 
            MapManager.ObjLink?.Map?.DungeonCastle == true));

        // Classic Camera transition speed loaded via "lahdmod".
        public float classic_transition_speed = 1.00f;

        public void LoadLAHDModFile()
        {
            // If a mod file exists load the values from it.
            string modFile = Path.Combine(Values.PathLAHDMods, "Camera.lahdmod");

            if (File.Exists(modFile))
                ModFile.Parse(modFile, this);
        }

        public void SetBounds(int viewportWidth, int viewportHeight)
        {
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;
        }

        public Rectangle GetCameraRectangle()
        {
            var rectangle = new Rectangle(
                RoundX - SnappedViewportWidth / 2,
                RoundY - SnappedViewportHeight / 2,
                SnappedViewportWidth, SnappedViewportHeight);
            return rectangle;
        }

        public Rectangle GetGameView()
        {
            var scale = ScaleValue;
            var rectangle = new Rectangle(
                (RoundX - SnappedViewportWidth / 2) / scale,
                (RoundY - SnappedViewportHeight / 2) / scale,
                SnappedViewportWidth / scale, SnappedViewportHeight / scale);
            return rectangle;
        }

        public Vector2 GetFieldCenter()
        {
            Vector2 fieldCoords = MapManager.ObjLink.CenterPosition.Position;
            var scale = ScaleValue;

            if (Game1.ClassicCamera.CameraFieldCoords != Vector2.Zero && !MapManager.ObjLink.Map.IsOverworld)
                fieldCoords = Game1.ClassicCamera.CameraFieldCoords;

            fieldRect = MapManager.ObjLink.Map.GetField(fieldCoords);

            int rectCenterX = (fieldRect.X + fieldRect.Width / 2) * scale;
            int rectCenterY = (fieldRect.Y + fieldRect.Height / 2) * scale;
            return new Vector2(rectCenterX, rectCenterY);
        }

        public void Center(Vector2 position, bool moveX, bool moveY)
        {
            // If SnapCamera was enabled and a timer started.
            if (SnapCameraTimer > 0)
                SnapCameraTimer -= Game1.DeltaTime;

            if (ClassicMode)
            {
                // Get the field rectangle and its center.
                Vector2 rectCenter = GetFieldCenter();

                // Snap when no smoothing, when snapping is enabled, or when the snap timer is set.
                if (!GameSettings.SmoothCamera || SnapCamera || SnapCameraTimer > 0)
                {
                    Location = rectCenter;
                    MoveLocation = rectCenter;
                    return;
                }
                if (MoveLocation != rectCenter)
                    MoveLocation = rectCenter;

                // Smoothly move camera toward MoveLocation.
                var direction = MoveLocation - Location;
                if (direction != Vector2.Zero)
                {
                    var distance = direction.Length() / Scale * CameraFollowMultiplier;
                    var speedMult = CameraFunction(distance / 3.5f);

                    direction.Normalize();
                    var cameraSpeed = direction * speedMult * Scale * Game1.TimeMultiplier * classic_transition_speed ;

                    if (moveX)
                        Location.X += cameraSpeed.X;
                    if (moveY)
                        Location.Y += cameraSpeed.Y;

                    // Snap if close enough.
                    if (distance <= 0.1f * Game1.TimeMultiplier)
                        Location = MoveLocation;
                }
                return;
            }
            else
            {
                if (SnapCamera || SnapCameraTimer > 0)
                {
                    Location = MapManager.GetCameraTargetLink();
                    MoveLocation = MapManager.GetCameraTargetLink();
                }
                if (!GameSettings.SmoothCamera)
                {
                    Location = position;
                    return;
                }
                var direction = position - MoveLocation;

                if (direction != Vector2.Zero)
                {
                    var distance = direction.Length() / Scale * CameraFollowMultiplier;
                    var speedMult = CameraFunction(distance / 12.5f);

                    direction.Normalize();
                    var cameraSpeed = direction * speedMult * Scale * Game1.TimeMultiplier;

                    if (moveX)
                        MoveLocation.X += cameraSpeed.X;
                    if (moveY)
                        MoveLocation.Y += cameraSpeed.Y;
                    if (distance <= 0.1f * Game1.TimeMultiplier)
                        MoveLocation = position;
                }
                // This is needed so the player does not wiggle around while the camera is following him.
                if (moveX)
                    _cameraDistance.X = position.X - MoveLocation.X;
                if (moveY)
                    _cameraDistance.Y = position.Y - MoveLocation.Y;
                Location = new Vector2((int)Math.Round(position.X), (int)Math.Round(position.Y)) - _cameraDistance;
            }
        }

        private float CameraFunction(float x)
        {
            var y = MathF.Atan(x);

            if (x > 2)
                y += (x - 2) / 2;

            return y + 0.1f;
        }

        public void ForceUpdate(Vector2 lockPosition)
        {
            MoveLocation = lockPosition;
            Location = lockPosition;
        }

        public void SoftUpdate(Vector2 position)
        {
            // When classic camera is enabled this will mess up transitions.
            if (ClassicMode && GameSettings.SmoothCamera)
                return;

            MoveLocation = position - _cameraDistance;
            Location = position;
        }

        public void OffsetCameraDistance(Vector2 offset)
        {
            _cameraDistance += offset;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Create a border around the current field.
            if (ClassicMode)
            {
                // Draw the black border even if using SGB border.
                if (GameSettings.ClassicBorder > 0)
                {
                    int thickness = 4;
                    int scale = ScaleValue;

                    // Screen center
                    var viewport = spriteBatch.GraphicsDevice.Viewport;
                    var screenCenter = new Vector2(viewport.Width / 2f, viewport.Height / 2f);

                    // Compute scaled field rect
                    var fieldX = fieldRect.X * scale - thickness;
                    var fieldY = fieldRect.Y * scale - thickness;
                    var fieldW = fieldRect.Width * scale + thickness * 2;
                    var fieldH = fieldRect.Height * scale + thickness * 2;

                    // Compute the field center in world space
                    var fieldCenter = new Vector2(
                        (fieldRect.X + fieldRect.Width / 2f) * scale,
                        (fieldRect.Y + fieldRect.Height / 2f) * scale
                    );

                    // Offset so that the field rect is centered on screen (like your camera)
                    var drawOffset = screenCenter - (fieldCenter - new Vector2(RoundX, RoundY));
                    var tex = Resources.SprWhite;

                    // Draw border lines (using alpha)
                    Color borderColor = Color.Black * GameSettings.ClassicAlpha;

                    // Draw Order: Top / Bottom / Left / Right
                    spriteBatch.Draw(tex, new Rectangle((int)(drawOffset.X + fieldX - RoundX), (int)(drawOffset.Y + fieldY - RoundY), (int)fieldW, thickness), borderColor);
                    spriteBatch.Draw(tex, new Rectangle((int)(drawOffset.X + fieldX - RoundX), (int)(drawOffset.Y + fieldY - RoundY + fieldH - thickness), (int)fieldW, thickness), borderColor);
                    spriteBatch.Draw(tex, new Rectangle((int)(drawOffset.X + fieldX - RoundX), (int)(drawOffset.Y + fieldY - RoundY - 1), thickness, (int)fieldH + 2), borderColor);
                    spriteBatch.Draw(tex, new Rectangle((int)(drawOffset.X + fieldX - RoundX + fieldW - thickness), (int)(drawOffset.Y + fieldY - RoundY - 1), thickness, (int)fieldH + 2), borderColor);

                    // Fill everything outside the border with black.
                    var screenW = viewport.Width + 1;
                    var screenH = viewport.Height + 1;

                    // Compute the rectangle's position on screen.
                    var rectScreenX = drawOffset.X + fieldX - RoundX;
                    var rectScreenY = drawOffset.Y + fieldY - RoundY;

                    // Draw border fill (using alpha)
                    Color blackoutColor = Color.Black * GameSettings.ClassicAlpha;

                    // Draw Order: Top / Bottom / Left / Right
                    spriteBatch.Draw(tex, new Rectangle(0, 0, screenW, (int)rectScreenY), blackoutColor);
                    spriteBatch.Draw(tex, new Rectangle(0, (int)(rectScreenY + fieldH), screenW, (int)(screenH - (rectScreenY + fieldH))), blackoutColor);
                    spriteBatch.Draw(tex, new Rectangle(0, (int)rectScreenY - 1, (int)rectScreenX, (int)fieldH + 2), blackoutColor);
                    spriteBatch.Draw(tex, new Rectangle((int)(rectScreenX + fieldW), (int)rectScreenY - 1, (int)(screenW - (rectScreenX + fieldW)), (int)fieldH + 2), blackoutColor);

                    // If set to the Super Game Boy border.
                    if (GameSettings.ClassicBorder == 2)
                    {
                        // Border's original pixel size.
                        const int borderW = 256;
                        const int borderH = 224;

                        // Scale by the camera scale (which should be integer scale with Classic Camera enabled).
                        int scaledW = (borderW * scale);
                        int scaledH = (borderH * scale);

                        // Center the SGB border on the screen.
                        Vector2 pos = new Vector2((viewport.Width  - scaledW) / 2f, (viewport.Height - scaledH) / 2f);

                        // Draw centered with point sampling.
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);

                        // Draw the border using nearest neighbor (point filtering) for sharp pixels).
                        spriteBatch.Draw(Resources.sgbBorder, pos, null, Color.White, 0f, Vector2.Zero, ScaleValue, SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}
