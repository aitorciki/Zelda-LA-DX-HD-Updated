using System;
using Microsoft.Xna.Framework;
using ProjectZ.Base;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.CObjects;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.Map;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.GameObjects.Things
{
    internal class ObjColliderOneWay : GameObject
    {
        private readonly Box _collisionBox;
        private readonly int _direction;
        private readonly bool _isPusher;

        public ObjColliderOneWay(Map.Map map, int posX, int posY, Rectangle collisionRectangle, Values.CollisionTypes type, int direction, bool isPusher) : base(map)
        {
            SprEditorImage = Resources.SprWhite;
            EditorIconSource = new Rectangle(0, 0, collisionRectangle.Width, collisionRectangle.Height);
            EditorColor = Color.DeepPink;

            EntityPosition = new CPosition(posX, posY, 0);
            EntitySize = collisionRectangle;

            _collisionBox = new Box(posX + collisionRectangle.X, posY + collisionRectangle.Y, 0, collisionRectangle.Width, collisionRectangle.Height, 3);
            _direction = direction;
            _isPusher = isPusher;

            AddComponent(CollisionComponent.Index, new CollisionComponent(CollisionCheck) { CollisionType = type });
        }

        private bool CollisionCheck(Box box, int dir, int level, ref Box collidingBox)
        {
            if (dir != _direction || !_collisionBox.Intersects(box))
                return false;

            if (_isPusher)
            {
                var Link = MapManager.ObjLink;
                var Body = Link._body;

                if (!Link.Is2DMode || Link.EntityPosition.Y <= _collisionBox.Y || Body.Velocity.Y <= 0)
                {
                    collidingBox = _collisionBox;
                    return true;
                }
                if (Math.Abs(Link.EntityPosition.Y - _collisionBox.Y) > 0.1f)
                    Link.SetPosition(new Vector2(Link.EntityPosition.X, _collisionBox.Y - 1));

                if (Link.CurrentState == ObjLink.State.Jumping)
                    Link.Animation.Play("walk_" + Link.Direction);
            }
            collidingBox = _collisionBox;
            return true;
        }
    }
}