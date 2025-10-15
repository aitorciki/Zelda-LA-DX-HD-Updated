using System;
using Microsoft.Xna.Framework;
using ProjectZ.Base;
using ProjectZ.InGame.Controls;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.GameObjects
{
    public partial class ObjLink
    {
        private Vector2 PreviousDirectionInput;

        private int ReverseDirection(int direction) => (direction + 2) % 4;

        private bool DestroyableWall(Box box)
        {
            _destroyableWallList.Clear();
            Map.Objects.GetComponentList(_destroyableWallList, (int)box.X, (int)box.Y, (int)box.Width + 1, (int)box.Height + 1, CollisionComponent.Mask);

            var collidingBox = Box.Empty;
            foreach (var gameObject in _destroyableWallList)
            {
                var collisionObject = gameObject.Components[CollisionComponent.Index] as CollisionComponent;
                if ((collisionObject.CollisionType & Values.CollisionTypes.Destroyable) != 0 &&
                    collisionObject.Collision(box, 0, 0, ref collidingBox))
                {
                    return true;
                }
            }
            return false;
        }

        private int ToDirection(Vector2 direction, bool modernAnalog = false, bool forceModern = false)
        {
            // If player wants old style movement.
            if (GameSettings.OldMovement && !forceModern)
                if (ControlHandler.LastDirectionDPad || (!modernAnalog && !ControlHandler.LastDirectionDPad))
                    return ToDirectionClassic(direction);

            // Fail safe in case the impossible happens.
            if (direction == Vector2.Zero) { return Direction; }

            // Get angle in degrees 0-360.
            float angle = (float)Math.Atan2(direction.Y, direction.X);
            float deg = MathHelper.ToDegrees(angle);
            if (deg < 0) { deg += 360f; }

            // 0:Left 1:Up 2:Right 3:Down
            if (deg >= 315 || deg < 45)   return 2;
            if (deg >= 45  && deg < 135)  return 3;
            if (deg >= 135 && deg < 225)  return 0;
            return 1;
        }

        private int ToDirectionClassic(Vector2 direction)
        {
            // No input — keep direction.
            if (direction == Vector2.Zero)
                return Direction;

            // Get angle in degrees 0-360.
            float angle = (float)Math.Atan2(direction.Y, direction.X);
            float deg = MathHelper.ToDegrees(angle);
            if (deg < 0) { deg += 360f; }

            // 0:Left 1:Up 2:Right 3:Down
            if (deg == 180) return 0;
            if (deg == 270) return 1;
            if (deg == 0)   return 2;
            if (deg == 90)  return 3;

            // Detect diagonal opposite movement (X and Y flipped signs).
            if (disable_moonwalk && GameMath.HasInvertedSigns(direction, PreviousDirectionInput))
            {
                // Flip to opposite direction.
                return Opposite(Direction);
            }
            // Otherwise, keep current direction.
            return Direction;
        }

        // Helper to return the opposite of a direction.
        private int Opposite(int dir)
        {
            // 0:Left 1:Up 2:Right 3:Down
            return dir switch
            {
                0 => 2,
                1 => 3,
                2 => 0,
                3 => 1,
                _ => dir
            };
        }
    }
}
