using Microsoft.Xna.Framework;
using ProjectZ.Base;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.CObjects;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.GameObjects.Things
{
    public class ObjFieldBarrier : GameObject
    {
        public Box CollisionBox;

        private int _width;
        private int _height;

        public ObjFieldBarrier(Map.Map map, int posX, int posY, Values.CollisionTypes type, Rectangle rectangle) : base(map)
        {
            _width  = rectangle.Width;
            _height = rectangle.Height;

            EditorIconSource = new Rectangle(0, 0, _width, _height);

            EntityPosition = new CPosition(posX, posY, 0);
            EntitySize = new Rectangle(0, 0, _width, _height);

            CollisionBox = new Box(posX + rectangle.X, posY + rectangle.Y, 0, _width, _height, 16);

            AddComponent(CollisionComponent.Index, new CollisionComponent(DetectCollision) { CollisionType = type });
        }

        public void SetPosition(int posX, int posY)
        {
            EntityPosition.Set(new Vector2(posX, posY));
            CollisionBox = new Box(posX, posY, 0, _width, _height, 16);
        }

        private bool DetectCollision(Box box, int dir, int level, ref Box collidingBox)
        {
            if (!CollisionBox.Intersects(box))
                return false;

            collidingBox = CollisionBox;
            return true;
        }
    }
}