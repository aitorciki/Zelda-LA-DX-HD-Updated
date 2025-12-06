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
        private string _strKey;
        private string _strValue;

        public ObjButtonPush(Map.Map map, int posX, int posY, string destroyKey, int destroyValue, int pushDirection, int buttonWidth, int buttonHeight) : base(map)
        {
            SprEditorImage = Resources.SprWhite;
            EditorIconSource = new Rectangle(0, 0, 16, 16);
            EditorColor = Color.Blue * 0.5f;

            _pushDirection = pushDirection;
            _strKey = destroyKey;
            _strValue = destroyValue.ToString();

            if (string.IsNullOrEmpty(_strKey) || Game1.GameManager.SaveManager.GetString(_strKey) == _strValue)
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
            // Remove the push button if the key value is set to 1.
            if (string.IsNullOrEmpty(_strKey) || Game1.GameManager.SaveManager.GetString(_strKey) == _strValue)
                Map.Objects.DeleteObjects.Add(this);
        }
    }
}