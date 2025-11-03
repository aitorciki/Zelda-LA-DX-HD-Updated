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

        public void FreezeGame(bool freeze)
        {
            if (freeze)
                Game1.GameManager.SaveManager.SetString("freezeGame", "1");
            else
                Game1.GameManager.SaveManager.SetString("freezeGame", "0");
        }

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

        private int ToDirection(Vector2 direction)
        {
            // Fail safe in case the impossible happens.
            if (direction == Vector2.Zero) { return Direction; }

            // If player wants old style movement.
            if (GameSettings.OldMovement)
                return ToDirectionClassic(direction);

            // Bias towards horizontal (0/2) or bias towards the vertical (1/3).
            float bias = (Direction == 0 || Direction == 2) ? 1.05f : 0.95f;

            // Prefer staying in current axis when movement is ambiguous.
            if (Math.Abs(direction.X) * bias > Math.Abs(direction.Y))
                return direction.X > 0 ? 2 : 0;
            else
                return direction.Y > 0 ? 3 : 1;
        }

        private int ToDirectionClassic(Vector2 direction)
        {
            // Get angle in degrees 0-360.
            float angle = (float)Math.Atan2(direction.Y, direction.X);
            float deg = MathHelper.ToDegrees(angle);
            if (deg < 0) { deg += 360f; }

            // 0:Left 1:Up 2:Right 3:Down
            return deg switch
            {
                180 => 0,
                270 => 1,
                0   => 2,
                90  => 3,
                _   => Direction
            };
        }
    }
}
