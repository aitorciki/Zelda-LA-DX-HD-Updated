using System;
using Microsoft.Xna.Framework;
using ProjectZ.Base;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.Map;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.GameObjects.Things
{
    internal class ObjButtonPush : GameObject
    {
        private readonly Box _collisionBox;

        private int _pushDirection;
        private string _destroyKey;
        private int _destroyValue;

        public ObjButtonPush(Map.Map map, int posX, int posY, string destroyKey, int destroyValue, int pushDirection, int buttonWidth, int buttonHeight) : base(map)
        {
            SprEditorImage = Resources.SprWhite;
            EditorIconSource = new Rectangle(0, 0, 16, 16);
            EditorColor = Color.Blue * 0.5f;

            _pushDirection = pushDirection;
            _destroyKey = destroyKey;
            _destroyValue = destroyValue;

            // Get the current value of the destroy key.
            int currentValue = 0;
            if (!string.IsNullOrEmpty(_destroyKey))
                currentValue = Convert.ToInt32(Game1.GameManager.SaveManager.GetString(_destroyKey));

            // Remove the push button if the current value is equal to or greater than the destroy value.
            if (currentValue >= _destroyValue)
            {
                IsDead = true;
                return;
            }
            _collisionBox = new Box(posX, posY, 0, buttonWidth, buttonHeight, 32);
            AddComponent(UpdateComponent.Index, new UpdateComponent(Update));
            AddComponent(KeyChangeListenerComponent.Index, new KeyChangeListenerComponent(OnKeyChange));
        }

        private void Update()
        {
            // The player walked into the push button collision box.
            if (_collisionBox.Intersects(MapManager.ObjLink._body.BodyBox.Box))
            {
                // Shorten the reference.
                var Link = MapManager.ObjLink;

                // Set the velocity, direction, and force walking.
                var velocity = AnimationHelper.DirectionOffset[_pushDirection];
                Link._body.VelocityTarget = velocity;
                Link.Direction = _pushDirection;
                Link.LinkWalking(true);

                // If charging the sword, correct the sword's direction.
                if (Link.IsChargingState(Link.CurrentState))
                    Link.PlayWeaponAnimation("stand_", _pushDirection);
            }
        }

        private void OnKeyChange()
        {
            // Get the current value of the destroy key.
            int currentValue = 0;
            if (!string.IsNullOrEmpty(_destroyKey))
                currentValue = Convert.ToInt32(Game1.GameManager.SaveManager.GetString(_destroyKey));

            // Remove the push button if the current value is equal to or greater than the destroy value.
            if (currentValue >= _destroyValue)
                Map.Objects.DeleteObjects.Add(this);
        }
    }
}