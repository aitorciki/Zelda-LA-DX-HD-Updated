using Microsoft.Xna.Framework;
using ProjectZ.Base;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.Map;
using ProjectZ.InGame.Things;
using System.Diagnostics;

namespace ProjectZ.InGame.GameObjects.Things
{
    internal class ObjButtonPush : GameObject
    {
        private readonly Box _collisionBox;

        private int _pushDirection;
        private string _strKey;

        public ObjButtonPush(Map.Map map, int posX, int posY, string destroyKey, int pushDirection, int buttonWidth, int buttonHeight) : base(map)
        {
            SprEditorImage = Resources.SprWhite;
            EditorIconSource = new Rectangle(0, 0, 16, 16);
            EditorColor = Color.Blue * 0.5f;

            _pushDirection = pushDirection;
            _strKey = destroyKey;

            if (string.IsNullOrEmpty(_strKey) || Game1.GameManager.SaveManager.GetString(destroyKey) == "1")
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
                // Set the velocity, direction, and force walking.
                var velocity = AnimationHelper.DirectionOffset[_pushDirection];
                MapManager.ObjLink._body.VelocityTarget = velocity;
                MapManager.ObjLink.Direction = _pushDirection;
                MapManager.ObjLink.LinkWalking(true);
            }
        }

        private void OnKeyChange()
        {
            // Remove the push button if the key value is set to 1.
            if (Game1.GameManager.SaveManager.GetString(_strKey, "0") == "1")
                Map.Objects.DeleteObjects.Add(this);
        }
    }
}