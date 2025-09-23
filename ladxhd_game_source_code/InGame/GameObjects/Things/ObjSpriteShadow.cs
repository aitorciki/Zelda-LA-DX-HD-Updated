using Microsoft.Xna.Framework;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.CObjects;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.GameObjects.Things
{
    internal class ObjSpriteShadow : GameObject
    {
        private DrawComponent _drawComponent;

        private GameObject _host;
        private CSprite _sprite;
        private CPosition _position;
        private Vector2 _offset;

        public ObjSpriteShadow() : base("sprshadow") { }

        public ObjSpriteShadow(GameObject host, float offsetX, float offsetY, Map.Map map) : base(map)
        {
            // Spawn in the shadow.
            Map.Objects.SpawnObject(this);

            // Grab the paramaeters.
            _host = host;
            _offset = new Vector2(offsetX, offsetY);

            // The initial spawn position.
            _position = new CPosition(_host.EntityPosition.Position.X + _offset.X, _host.EntityPosition.Position.Y + _offset.Y, 0);
            _sprite = new CSprite("sprshadow", _position);
            EntityPosition = _position;

            // Attach the sprite shadow to the game object.
            AddComponent(DrawComponent.Index, _drawComponent = new DrawCSpriteComponent(_sprite, Values.LayerPlayer));
            AddComponent(UpdateComponent.Index, new UpdateComponent(Update));
        }

        private void Update()
        {
            // For Link, we can just remove the shadow in "Update3D". But for other objects, it won't be easy to remove
            // the shadow object so just don't draw it and don't update the position if "Enable Shadows" is disabled.
            _drawComponent.IsActive = !GameSettings.EnableShadows;

            if (!GameSettings.EnableShadows)
                UpdatePosition();
        }

        private void UpdatePosition()
        {
            _position = new CPosition(_host.EntityPosition.Position.X + _offset.X, _host.EntityPosition.Position.Y + _offset.Y, 0);
            EntityPosition.Set(_position.Position);
        }
    }
}