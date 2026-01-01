using Microsoft.Xna.Framework;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.CObjects;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.GameObjects.Base.Systems;
using ProjectZ.InGame.GameObjects.NPCs;
using ProjectZ.InGame.Map;
using ProjectZ.InGame.SaveLoad;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.GameObjects.Things
{
    internal class ObjBowWowWater : GameObject
    {
        private readonly Animator _animator;
        private readonly DrawComponent _drawComponent;
        public CSprite _sprite;
        public CPosition _position;

        public ObjBowWow _host;
        private Vector2 _offset = new Vector2(-3, 11);

        public ObjBowWowWater() : this("bowwow_water") { }

        public ObjBowWowWater(string spriteName) : base(spriteName) { }

        public ObjBowWowWater(Map.Map map, float posX, float posY, ObjBowWow host) : base(map)
        {
            EntityPosition = new CPosition(posX + _offset.X, posY + _offset.Y, 0);

            _host = host;

            var _sprite = new CSprite(EntityPosition);

            _animator = AnimatorSaveLoad.LoadAnimator("NPCs/bowwow_water");
            var animationComponent = new AnimationComponent(_animator, _sprite, new Vector2(-8, -15));

            AddComponent(BaseAnimationComponent.Index, animationComponent);
            AddComponent(DrawComponent.Index, _drawComponent = new DrawCSpriteComponent(_sprite, Values.LayerBottom));
            AddComponent(UpdateComponent.Index, new UpdateComponent(Update));

            Map.Objects.SpawnObject(this);
            Map.Objects.RegisterAlwaysAnimateObject(this);

            _animator.Play("stand");
        }

        private void Update()
        {
            // Update the position of the water effect.
            _position = new CPosition(_host.EntityPosition.Position.X + _offset.X, _host.EntityPosition.Position.Y + _offset.Y, 0);
            EntityPosition.Set(_position.Position);

            // Check if Bow Wow is currently in the water.
            bool inWater = SystemBody.GetFieldState(_host._body).HasFlag(MapStates.FieldStates.DeepWater);

            // Get the water state and see if he's jumping or not.
            if (inWater && _host.EntityPosition.Z <= 2.5f)
                _drawComponent.IsActive = true;
            else
                _drawComponent.IsActive = false;

            // If the map is null destroy the object.
            if (Map == null || _host.Map == null)
                Destroy();
        }

        public void Destroy()
        {
            _drawComponent.IsActive = false;
            Map.Objects.RemoveObject(this);
        }
    }
}