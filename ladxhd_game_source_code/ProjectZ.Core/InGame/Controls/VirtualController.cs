using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using ProjectZ.Base;
using ProjectZ.InGame.SaveLoad;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.Controls
{
    public static class VirtualController
    {
        private static readonly List<VirtualButton> _buttons = new List<VirtualButton>();
        private static VirtualStick _leftStick;
        private static VirtualStick _rightStick;

        private static DictAtlasEntry _dPadSprite;
        private static Rectangle _dPadBounds;

        private static float ControlsScale => GameSettings.TouchScaling * 0.25f;

        public static float ButtonMinAlpha = 0.30f;
        public static float ButtonMaxAlpha = 0.85f;

        public static float ShadowMinAlpha = 0.15f;
        public static float ShadowMaxAlpha = 0.30f;

        public static float DPadButtonAlpha = ButtonMinAlpha;
        public static float DPadShadowAlpha = ShadowMaxAlpha;

        public static VirtualStick GetLeftStick() => _leftStick;
        public static VirtualStick GetRightStick() => _rightStick;
        public static List<VirtualButton> GetButtons() => _buttons;

        public static Vector2 GetLeftStickOutput() => _leftStick != null ? _leftStick.Output : Vector2.Zero;
        public static Vector2 GetRightStickOutput() => _rightStick != null ? _rightStick.Output : Vector2.Zero;

        private static Point GetShadowOffset()
        {
            int offset = (int)(3 * ControlsScale);
            return new Point(offset, offset);
        }

        public static void Initialize(int screenWidth, int screenHeight)
        {
            _buttons.Clear();

            float scale = ControlsScale;
            int buttonSize = (int)(40 * scale);
            int margin = (int)(16 * scale);
            int spacing = (int)(8 * scale);

            // ------------------------------------------------------------------------------------------------------------------------
            // LEFT SIDE: DPAD 
            // ------------------------------------------------------------------------------------------------------------------------
            int leftX = margin;
            int leftY = screenHeight - margin;

            _dPadSprite = Resources.GetSprite("button_dpad");
            _dPadBounds = new Rectangle(leftX, leftY - (buttonSize * 3) - (spacing * 2), buttonSize * 3 + spacing * 2, buttonSize * 3 + spacing * 2);

            Rectangle rectLeft = new Rectangle(leftX, leftY - (buttonSize * 2) - spacing, buttonSize, buttonSize);
            Rectangle rectUp = new Rectangle(leftX + buttonSize + spacing, leftY - (buttonSize * 3) - (spacing * 2), buttonSize, buttonSize);
            Rectangle rectRight = new Rectangle(leftX + (buttonSize * 2) + (spacing * 2), leftY - (buttonSize * 2) - spacing, buttonSize, buttonSize);
            Rectangle rectDown = new Rectangle(leftX + buttonSize + spacing, leftY - buttonSize, buttonSize, buttonSize);

            _buttons.Add(new VirtualButton("null", CButtons.Left, rectLeft));
            _buttons.Add(new VirtualButton("null", CButtons.Up, rectUp));
            _buttons.Add(new VirtualButton("null", CButtons.Right, rectRight));
            _buttons.Add(new VirtualButton("null", CButtons.Down, rectDown));

            Rectangle rectUpLeft = new Rectangle(leftX, leftY - (buttonSize * 3) - (spacing * 2), buttonSize, buttonSize);
            Rectangle rectUpRight = new Rectangle(leftX + (buttonSize * 2) + (spacing * 2), leftY - (buttonSize * 3) - (spacing * 2), buttonSize, buttonSize);
            Rectangle rectDownLeft = new Rectangle(leftX, leftY - buttonSize, buttonSize, buttonSize);
            Rectangle rectDownRight = new Rectangle(leftX + (buttonSize * 2) + (spacing * 2), leftY - buttonSize, buttonSize, buttonSize);

            _buttons.Add(new VirtualButton("null", CButtons.Up | CButtons.Left, rectUpLeft));
            _buttons.Add(new VirtualButton("null", CButtons.Up | CButtons.Right, rectUpRight));
            _buttons.Add(new VirtualButton("null", CButtons.Down | CButtons.Left, rectDownLeft));
            _buttons.Add(new VirtualButton("null", CButtons.Down | CButtons.Right, rectDownRight));

            // ------------------------------------------------------------------------------------------------------------------------
            // RIGHT SIDE: X / Y / B / A
            // ------------------------------------------------------------------------------------------------------------------------
            int rightX = screenWidth - margin;
            int rightY = screenHeight - margin;

            Rectangle rectX = new Rectangle(rightX - (buttonSize * 3) - (spacing * 2), rightY - (buttonSize * 2) - spacing, buttonSize, buttonSize);
            Rectangle rectY = new Rectangle(rightX - (buttonSize * 2) - spacing, rightY - (buttonSize * 3) - (spacing * 2), buttonSize, buttonSize);
            Rectangle rectB = new Rectangle(rightX - buttonSize, rightY - (buttonSize * 2) - spacing, buttonSize, buttonSize);
            Rectangle rectA = new Rectangle(rightX - (buttonSize * 2) - spacing, rightY - buttonSize, buttonSize, buttonSize);

            _buttons.Add(new VirtualButton("button_x", CButtons.X, rectX));
            _buttons.Add(new VirtualButton("button_y", CButtons.Y, rectY));
            _buttons.Add(new VirtualButton("button_b", CButtons.B, rectB));
            _buttons.Add(new VirtualButton("button_a", CButtons.A, rectA));

            // ------------------------------------------------------------------------------------------------------------------------
            // STICK BUTTONS
            // ------------------------------------------------------------------------------------------------------------------------
            if (GameSettings.TouchSticks)
            {
                int extraButtonLift = GameSettings.SixButtons ? buttonSize + spacing : spacing;
                Rectangle rectExtraR = new Rectangle(rectY.X, rectY.Y - buttonSize - spacing - extraButtonLift, buttonSize, buttonSize);
                _buttons.Add(new VirtualButton("button_rc", CButtons.RS, rectExtraR));

                int dpadCenterX = _dPadBounds.X + (_dPadBounds.Width / 2) - (buttonSize / 2);
                Rectangle rectExtraL = new Rectangle(dpadCenterX, _dPadBounds.Y - (buttonSize * 2) - spacing, buttonSize, buttonSize);
                _buttons.Add(new VirtualButton("button_lc", CButtons.LS, rectExtraL));
            }
            // ------------------------------------------------------------------------------------------------------------------------
            // ANALOG STICKS
            // ------------------------------------------------------------------------------------------------------------------------
            float stickRadius = 40f * scale;

            float clusterWidth = buttonSize * 3 + spacing * 2;
            float stickGap = spacing * 2 + stickRadius;
            int stickLift = (int)(20 * scale);

            Vector2 leftStickCenter = new Vector2(leftX + clusterWidth + stickGap, screenHeight - margin - stickRadius - stickLift);
            Vector2 rightStickCenter = new Vector2(rightX - clusterWidth - stickGap, screenHeight - margin - stickRadius - stickLift);

            _leftStick  = new VirtualStick("button_ls", leftStickCenter, stickRadius);
            _rightStick = new VirtualStick("button_rs", rightStickCenter, stickRadius);

            // ------------------------------------------------------------------------------------------------------------------------
            // TOP SHOULDER BUTTONS / SIX BUTTON LAYOUT
            // ------------------------------------------------------------------------------------------------------------------------
            int topY = margin;

            if (GameSettings.SixButtons)
            {
                Rectangle rectLB6 = new Rectangle(rectX.X, rectY.Y - buttonSize - spacing, buttonSize, buttonSize);
                Rectangle rectRB6 = new Rectangle(rectB.X, rectY.Y - buttonSize - spacing, buttonSize, buttonSize);

                _buttons.Add(new VirtualButton("button_lb", CButtons.LB, rectLB6));
                _buttons.Add(new VirtualButton("button_rb", CButtons.RB, rectRB6));
                _buttons.Add(new VirtualButton("button_lt", CButtons.LT, new Rectangle(margin, topY, buttonSize, buttonSize)));
                _buttons.Add(new VirtualButton("button_rt", CButtons.RT, new Rectangle(screenWidth - margin - buttonSize, topY, buttonSize, buttonSize)));
            }
            else
            {
                _buttons.Add(new VirtualButton("button_lt", CButtons.LT, new Rectangle(margin, topY, buttonSize, buttonSize)));
                _buttons.Add(new VirtualButton("button_lb", CButtons.LB, new Rectangle(margin + buttonSize + (spacing * 2), topY, buttonSize, buttonSize)));
                _buttons.Add(new VirtualButton("button_rt", CButtons.RT, new Rectangle(screenWidth - margin - buttonSize, topY, buttonSize, buttonSize)));
                _buttons.Add(new VirtualButton("button_rb", CButtons.RB, new Rectangle(screenWidth - margin - (buttonSize * 2) - (spacing * 2), topY, buttonSize, buttonSize)));
            }
            // ------------------------------------------------------------------------------------------------------------------------
            // SELECT / START
            // ------------------------------------------------------------------------------------------------------------------------
            int centerX = screenWidth / 2;
            int bottomY = screenHeight - margin - buttonSize;

            if (GameSettings.TouchTopMiddle)
            {
                _buttons.Add(new VirtualButton("button_share", CButtons.Select, new Rectangle(centerX - buttonSize - spacing, topY, buttonSize, buttonSize)));
                _buttons.Add(new VirtualButton("button_menu", CButtons.Start, new Rectangle(centerX + spacing, topY, buttonSize, buttonSize)));
            }
            else
            {
                _buttons.Add(new VirtualButton("button_share", CButtons.Select, new Rectangle(centerX - buttonSize - spacing, bottomY, buttonSize, buttonSize)));
                _buttons.Add(new VirtualButton("button_menu", CButtons.Start, new Rectangle(centerX + spacing, bottomY, buttonSize, buttonSize)));
            }
        }

        public static void UpdateButtonsAlpha()
        {
            foreach (var button in _buttons)
            {
                button.DisplayAlpha = UpdateButtonAlpha(button.DisplayAlpha, button.IsDown);
                button.ShadowAlpha  = UpdateShadowAlpha(button.ShadowAlpha, button.IsDown);
            }
            if (_leftStick != null)
            {
                _leftStick.DisplayAlpha = UpdateButtonAlpha(_leftStick.DisplayAlpha, _leftStick.IsDown);
                _leftStick.ShadowAlpha  = UpdateShadowAlpha(_leftStick.ShadowAlpha, _leftStick.IsDown);
            }
            if (_rightStick != null)
            {
                _rightStick.DisplayAlpha = UpdateButtonAlpha(_rightStick.DisplayAlpha, _rightStick.IsDown);
                _rightStick.ShadowAlpha  = UpdateShadowAlpha(_rightStick.ShadowAlpha, _rightStick.IsDown);
            }
            bool dPadActive = DPadIsActive();
            DPadButtonAlpha = UpdateButtonAlpha(DPadButtonAlpha, dPadActive);
            DPadShadowAlpha = UpdateShadowAlpha(DPadShadowAlpha, dPadActive);
        }

        private static float UpdateButtonAlpha(float currentAlpha, bool isActive)
        {
            float targetAlpha;

            if (GameSettings.TouchControls == 0)
                targetAlpha = 0f;
            else if (GameSettings.TouchControls == 2)
                targetAlpha = ButtonMaxAlpha;
            else
                targetAlpha = isActive ? ButtonMaxAlpha : ButtonMinAlpha;

            float speed = 0.01f * Game1.DeltaTime;
            return MathHelper.Lerp(currentAlpha, targetAlpha, MathHelper.Clamp(speed, 0f, 1f));
        }

        private static float UpdateShadowAlpha(float currentAlpha, bool isActive)
        {
            float targetAlpha;

            if (GameSettings.TouchControls == 0)
                targetAlpha = 0f;
            else if (GameSettings.TouchControls == 2)
                targetAlpha = ShadowMaxAlpha;
            else
                targetAlpha = isActive ? ShadowMaxAlpha : ShadowMinAlpha;

            float speed = 0.01f * Game1.DeltaTime;
            return MathHelper.Lerp(currentAlpha, targetAlpha, MathHelper.Clamp(speed, 0f, 1f));
        }

    #if ANDROID
        public static void Update()
        {
            foreach (var button in _buttons)
                button.BeginUpdate();

            _leftStick?.BeginUpdate();
            _rightStick?.BeginUpdate();

            if (GameSettings.TouchControls == 0)
            {
                foreach (var button in _buttons)
                    button.TouchId = null;

                if (_leftStick != null)
                    _leftStick.TouchId = null;

                if (_rightStick != null)
                    _rightStick.TouchId = null;

                // Update the alpha for all buttons.
                UpdateButtonsAlpha();
                return;
            }
        
            var touches = InputHandler.TouchState;
            int holdPadding = (int)(12 * ControlsScale);

            // First pass:
            // Keep already-owned buttons alive if their finger is still active.
            foreach (var button in _buttons)
            {
                if (button.TouchId == null)
                    continue;

                bool foundTouch = false;

                for (int i = 0; i < touches.Count; i++)
                {
                    TouchLocation touch = touches[i];

                    if (touch.Id != button.TouchId.Value)
                        continue;

                    if (touch.State == TouchLocationState.Released ||
                        touch.State == TouchLocationState.Invalid)
                        break;

                    foundTouch = true;

                    Point point = touch.Position.ToPoint();

                    if (button.ContainsExpanded(point, holdPadding))
                        button.IsDown = true;

                    break;
                }
                if (!foundTouch || !button.IsDown)
                    button.TouchId = null;
            }

            // Keep left stick alive if its finger is still active.
            if (_leftStick != null && _leftStick.TouchId != null)
            {
                bool foundTouch = false;

                for (int i = 0; i < touches.Count; i++)
                {
                    TouchLocation touch = touches[i];

                    if (touch.Id != _leftStick.TouchId.Value)
                        continue;

                    if (touch.State == TouchLocationState.Released ||
                        touch.State == TouchLocationState.Invalid)
                        break;

                    foundTouch = true;
                    _leftStick.IsDown = true;
                    _leftStick.SetTouchPosition(touch.Position);
                    break;
                }

                if (!foundTouch || !_leftStick.IsDown)
                    _leftStick.TouchId = null;
            }

            // Keep right stick alive if its finger is still active.
            if (_rightStick != null && _rightStick.TouchId != null)
            {
                bool foundTouch = false;

                for (int i = 0; i < touches.Count; i++)
                {
                    TouchLocation touch = touches[i];

                    if (touch.Id != _rightStick.TouchId.Value)
                        continue;

                    if (touch.State == TouchLocationState.Released ||
                        touch.State == TouchLocationState.Invalid)
                        break;

                    foundTouch = true;
                    _rightStick.IsDown = true;
                    _rightStick.SetTouchPosition(touch.Position);
                    break;
                }

                if (!foundTouch || !_rightStick.IsDown)
                    _rightStick.TouchId = null;
            }

            // Second pass:
            // Any unclaimed active touch can claim a free button or free stick.
            for (int i = 0; i < touches.Count; i++)
            {
                TouchLocation touch = touches[i];

                if (touch.State != TouchLocationState.Pressed &&
                    touch.State != TouchLocationState.Moved)
                    continue;

                bool alreadyUsed = false;

                foreach (var button in _buttons)
                {
                    if (button.TouchId == touch.Id)
                    {
                        alreadyUsed = true;
                        break;
                    }
                }

                if (!alreadyUsed && _leftStick != null && _leftStick.TouchId == touch.Id)
                    alreadyUsed = true;

                if (!alreadyUsed && _rightStick != null && _rightStick.TouchId == touch.Id)
                    alreadyUsed = true;

                if (alreadyUsed)
                    continue;

                Point point = touch.Position.ToPoint();

                // Try buttons first.
                foreach (var button in _buttons)
                {
                    if (button.TouchId != null)
                        continue;

                    if (button.Contains(point))
                    {
                        button.IsDown = true;
                        button.TouchId = touch.Id;
                        alreadyUsed = true;
                        break;
                    }
                }

                if (alreadyUsed)
                    continue;

                // Then try left stick.
                if (_leftStick != null && _leftStick.TouchId == null && _leftStick.Contains(point))
                {
                    _leftStick.IsDown = true;
                    _leftStick.TouchId = touch.Id;
                    _leftStick.SetTouchPosition(touch.Position);
                    continue;
                }

                // Then try right stick.
                if (_rightStick != null && _rightStick.TouchId == null && _rightStick.Contains(point))
                {
                    _rightStick.IsDown = true;
                    _rightStick.TouchId = touch.Id;
                    _rightStick.SetTouchPosition(touch.Position);
                }
            }
            // Update the alpha for all buttons.
            UpdateButtonsAlpha();
        }
    #endif

        public static bool ButtonDown(CButtons button)
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                if ((_buttons[i].Button & button) != 0 && _buttons[i].IsDown)
                    return true;
            }

            return false;
        }

        public static bool ButtonPressed(CButtons button)
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                if ((_buttons[i].Button & button) != 0 && _buttons[i].Pressed())
                    return true;
            }
            return false;
        }

        public static bool ButtonReleased(CButtons button)
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                if ((_buttons[i].Button & button) != 0 && _buttons[i].Released())
                    return true;
            }
            return false;
        }

        private static bool DPadIsActive()
        {
            return ButtonDown(CButtons.Left) || ButtonDown(CButtons.Up) || ButtonDown(CButtons.Right) || ButtonDown(CButtons.Down);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (GameSettings.TouchControls == 0)
                return;

            if (_dPadSprite != null)
            {
                float alpha = DPadButtonAlpha;
                float shadowAlpha = DPadShadowAlpha;
                Point shadowOffset = GetShadowOffset();

                Rectangle src = _dPadSprite.ScaledRectangle;
                Vector2 position = new Vector2(_dPadBounds.X, _dPadBounds.Y);
                Vector2 shadowPosition = new Vector2(_dPadBounds.X + shadowOffset.X, _dPadBounds.Y + shadowOffset.Y);

                float scaleX = _dPadBounds.Width / (float)src.Width;
                float scaleY = _dPadBounds.Height / (float)src.Height;
                Vector2 scale = new Vector2(scaleX, scaleY);

                spriteBatch.Draw(_dPadSprite.Texture, shadowPosition, src, Color.Black * shadowAlpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(_dPadSprite.Texture, position, src, Color.White * alpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }

            DrawStick(spriteBatch, _leftStick);
            DrawStick(spriteBatch, _rightStick);

            for (int i = 0; i < _buttons.Count; i++)
            {
                var button = _buttons[i];
                float alpha = button.DisplayAlpha;

                if (button.Sprite != null)
                {
                    float shadowAlpha = button.ShadowAlpha;
                    Point shadowOffset = GetShadowOffset();

                    Rectangle src = button.Sprite.ScaledRectangle;
                    Vector2 position = new Vector2(button.Bounds.X, button.Bounds.Y);
                    Vector2 shadowPosition = new Vector2(button.Bounds.X + shadowOffset.X, button.Bounds.Y + shadowOffset.Y);

                    float scaleX = button.Bounds.Width / (float)src.Width;
                    float scaleY = button.Bounds.Height / (float)src.Height;
                    Vector2 scale = new Vector2(scaleX, scaleY);

                    spriteBatch.Draw(button.Sprite.Texture, shadowPosition, src, Color.Black * shadowAlpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(button.Sprite.Texture, position, src, Color.White * alpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
                else if (button.SpriteName != "null")
                {
                    spriteBatch.Draw(Resources.SprWhite, button.Bounds, Color.White * alpha);
                }
            }
        }

        private static void DrawStick(SpriteBatch spriteBatch, VirtualStick stick)
        {
            if (stick == null)
                return;

            int baseSize = (int)(stick.Radius * 2f);
            int knobSize = (int)(stick.Radius * 1.1f);

            Rectangle baseRect = new Rectangle((int)(stick.Center.X - stick.Radius), (int)(stick.Center.Y - stick.Radius), baseSize, baseSize);
            Rectangle knobRect = new Rectangle((int)(stick.KnobPosition.X - knobSize / 2f), (int)(stick.KnobPosition.Y - knobSize / 2f), knobSize, knobSize);

            float baseAlpha = stick.DisplayAlpha;
            float knobAlpha = stick.DisplayAlpha;

            Point shadowOffset = GetShadowOffset();

            if (stick.BaseSprite != null)
            {
                Rectangle src = stick.BaseSprite.ScaledRectangle;
                float scaleX = baseRect.Width / (float)src.Width;
                float scaleY = baseRect.Height / (float)src.Height;
                Vector2 scale = new Vector2(scaleX, scaleY);

                Vector2 basePosition = new Vector2(baseRect.X, baseRect.Y);
                Vector2 baseShadowPosition = new Vector2(baseRect.X + shadowOffset.X, baseRect.Y + shadowOffset.Y);

                spriteBatch.Draw(stick.BaseSprite.Texture, baseShadowPosition, src, Color.Black * stick.ShadowAlpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(stick.BaseSprite.Texture, basePosition, src, Color.White * baseAlpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(Resources.SprWhite, baseRect, Color.White * baseAlpha);
            }
            // Draw moving head texture
            if (stick.HeadSprite != null)
            {
                Rectangle src = stick.HeadSprite.ScaledRectangle;
                float scaleX = knobRect.Width / (float)src.Width;
                float scaleY = knobRect.Height / (float)src.Height;
                Vector2 scale = new Vector2(scaleX, scaleY);

                Vector2 knobPosition = new Vector2(knobRect.X, knobRect.Y);
                Vector2 knobShadowPosition = new Vector2(knobRect.X + shadowOffset.X, knobRect.Y + shadowOffset.Y);

                spriteBatch.Draw(stick.HeadSprite.Texture, knobShadowPosition, src, Color.Black * stick.ShadowAlpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(stick.HeadSprite.Texture, knobPosition, src, Color.White * knobAlpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(Resources.SprWhite, knobRect, Color.White * knobAlpha);
            }
        }
    }
}