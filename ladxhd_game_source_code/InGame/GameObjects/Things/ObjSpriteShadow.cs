using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
        private UpdateComponent _updateComponent;

        private GameObject _host;
        private CSprite _sprite;
        private CPosition _position;
        private int _layer;

        private Vector2 _offset = new Vector2(-8, -14);

        public ObjSpriteShadow() : base("sprshadow") { }

        public ObjSpriteShadow(GameObject host, int layer, float offsetX, float offsetY, Map.Map map) : this(host, layer, map)
        {
            _offset = new Vector2(offsetX, offsetY);
        }

        public ObjSpriteShadow(GameObject host, int layer, Map.Map map) : base(map)
        {
            // A failsafe to prevent crashes. This shouldn't happen but it could.
            if (_host == null && map == null) return;

            Map.Objects.SpawnObject(this);

            _host = host;
            _layer = layer;
            _position = new CPosition(_host.EntityPosition.Position.X + _offset.X, _host.EntityPosition.Position.Y + _offset.Y, 0);
            _sprite = new CSprite("sprshadow", _position);

            EntityPosition = _position;

            AddComponent(DrawComponent.Index, _drawComponent = new DrawCSpriteComponent(_sprite, _layer));
            AddComponent(UpdateComponent.Index, _updateComponent = new UpdateComponent(Update));
        }

        private void Update()
        {
            _drawComponent.IsActive = !GameSettings.EnableShadows;

            if (!GameSettings.EnableShadows)
                UpdatePosition();

            if (_host is IHasVisibility hostVisbility)
                _sprite.IsVisible = hostVisbility.IsVisible;

            if (_host.Map == null)
                Destroy();
        }

        private void UpdatePosition()
        {
            _position = new CPosition(_host.EntityPosition.Position.X + _offset.X, _host.EntityPosition.Position.Y + _offset.Y, 0);
            EntityPosition.Set(_position.Position);
        }

        private void Destroy()
        {
            _drawComponent.IsActive = false;
            _updateComponent.IsActive = false;
            Map.Objects.RemoveObject(this);
        }
    }
}