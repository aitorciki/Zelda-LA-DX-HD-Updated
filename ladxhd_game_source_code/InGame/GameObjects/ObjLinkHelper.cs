using Microsoft.Xna.Framework;
using ProjectZ.Base;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.Things;
using System;

namespace ProjectZ.InGame.GameObjects
{
    public partial class ObjLink
    {
        /// <summary>
        /// Check if the box is colliding with a destroyable wall
        /// </summary>
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

        /// <summary>
        /// Get the direction with a tolerance in the current direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private int ToDirection(Vector2 direction)
        {
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
    }
}
