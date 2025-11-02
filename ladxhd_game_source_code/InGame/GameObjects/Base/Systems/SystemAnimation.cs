using System;
using System.Collections.Generic;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.GameObjects.Base.Pools;
using ProjectZ.InGame.Map;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.GameObjects.Base.Systems
{
    class SystemAnimation
    {
        public ComponentPool Pool;

        private readonly List<GameObject> _objectList = new List<GameObject>();

        public void Update(bool dialogOpen, Type[] objectTypes = null)
        {
            _objectList.Clear();

            // Only update objects that are within the current field.
            if (GameSettings.ClassicCamera)
            {
                Pool.GetComponentList(_objectList, ObjectManager.UpdateField.X, ObjectManager.UpdateField.Y, 
                    ObjectManager.UpdateField.Width, ObjectManager.UpdateField.Height, BaseAnimationComponent.Mask);
                _objectList.RemoveAll(o => o.EntityPosition != null && !ObjectManager.ActualField.Contains(o.EntityPosition.Position));

                // Make sure that Link's follower is always updated.
                if (!_objectList.Contains(MapManager.ObjLink._objFollower) && MapManager.ObjLink._objFollower != null)
                    _objectList.Add(MapManager.ObjLink._objFollower);

                // Always update the boomerang as well.
                if (!_objectList.Contains(MapManager.ObjLink.Boomerang) && MapManager.ObjLink.Boomerang != null)
                    _objectList.Add(MapManager.ObjLink.Boomerang);
            }
            // Only update the objects that are currently visible.
            else
            {
                Pool.GetComponentList(_objectList,
                    (int)((MapManager.Camera.X - Game1.RenderWidth / 2) / MapManager.Camera.Scale),
                    (int)((MapManager.Camera.Y - Game1.RenderHeight / 2) / MapManager.Camera.Scale),
                    (int)(Game1.RenderWidth / MapManager.Camera.Scale),
                    (int)(Game1.RenderHeight / MapManager.Camera.Scale), BaseAnimationComponent.Mask);
            }
            foreach (var gameObject in _objectList)
            {
                bool skipObject = (objectTypes == null) switch
                {
                    true  => (!gameObject.IsActive),
                    false => (!gameObject.IsActive || !ObjectManager.IsGameObjectType(gameObject, objectTypes))
                };
                if (!gameObject.IsActive || skipObject) { continue; }
                var animationComponent = gameObject.Components[BaseAnimationComponent.Index] as BaseAnimationComponent;
                if (animationComponent == null) { continue; }
                if (!dialogOpen || animationComponent.UpdateWithOpenDialog)
                    animationComponent.UpdateAnimation();
            }
        }
    }
}
