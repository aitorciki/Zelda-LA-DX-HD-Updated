using Microsoft.Xna.Framework;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.CObjects;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.GameObjects.Things
{
    internal class ObjHole : GameObject
    {
        private readonly DrawSpriteComponent _drawComponent;
        private readonly BoxCollisionComponent _collisionComponent;

        public readonly Vector2 Center;
        public readonly int Color;
        public CBox collisionBox;

        public ObjHole() : base("hole_0") { }

        public ObjHole(Map.Map map, int posX, int posY, int width, int height, Rectangle sourceRectangle, int offsetX, int offsetY, int color) : base(map)
        {
            Tags = Values.GameObjectTag.Hole;

            Center = new Vector2(posX + offsetX + width / 2, posY + offsetY + height / 2);
            Color = color;

            if (sourceRectangle == Rectangle.Empty)
            {
                EntityPosition = new CPosition(posX + offsetX, posY + offsetY, 0);
                EntitySize = new Rectangle(0, 0, width, height);
            }
            else
            {
                EntityPosition = new CPosition(posX, posY, 0);
                EntitySize = new Rectangle(0, 0, sourceRectangle.Width, sourceRectangle.Height);
            }
            float holePosX = posX + offsetX;
            float holePosY = posY + offsetY;
            float holeWidth = width;
            float holeHeight = height;

            collisionBox = new CBox(holePosX, holePosY, 0, holeWidth, holeHeight, 16);
            _collisionComponent = new BoxCollisionComponent(collisionBox, Values.CollisionTypes.Hole);
            AddComponent(CollisionComponent.Index, _collisionComponent);

            // visible hole?
            if (sourceRectangle != Rectangle.Empty)
            {
                _drawComponent = new DrawSpriteComponent(Resources.SprObjects, EntityPosition, sourceRectangle, new Vector2(0, 0), Values.LayerBottom);
                AddComponent(DrawComponent.Index, _drawComponent);
            }
        }

        public void SetActive(bool state)
        {
            if (_drawComponent != null)
                _drawComponent.IsActive = state;
            _collisionComponent.IsActive = state;
        }
    }
}