using System;
using System.Collections.Generic;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.GameObjects.Base.Pools;
using ProjectZ.InGame.Map;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.GameObjects.Base.Systems
{
    class SystemAi
    {
        public ComponentPool Pool;

        private readonly List<GameObject> _objectList = new List<GameObject>();

        public void Update(Type[] alwaysAnimateTypes = null)
        {
            _objectList.Clear();

            // Only update objects that are within the current field.
            if (Camera.ClassicMode)
            {
                Pool.GetComponentList(_objectList, ObjectManager.UpdateField.X, ObjectManager.UpdateField.Y, 
                    ObjectManager.UpdateField.Width, ObjectManager.UpdateField.Height, AiComponent.Mask);
                _objectList.RemoveAll(o => o.EntityPosition != null && !ObjectManager.ActualField.Contains(o.EntityPosition.Position));

                // Make sure that Link's follower is always updated.
                if (!_objectList.Contains(MapManager.ObjLink._objFollower) && MapManager.ObjLink._objFollower != null)
                    _objectList.Add(MapManager.ObjLink._objFollower);

                // Always update the boomerang as well.
                if (!_objectList.Contains(MapManager.ObjLink.Boomerang) && MapManager.ObjLink.Boomerang != null)
                    _objectList.Add(MapManager.ObjLink.Boomerang);

                // If Link currently has BowWow with him he needs updated.
                if (!_objectList.Contains(MapManager.ObjLink._objBowWow) && MapManager.ObjLink._objBowWow != null)
                    _objectList.Add(MapManager.ObjLink._objBowWow);
            }
            // Only update the objects that are currently visible.
            else
            {
                Pool.GetComponentList(_objectList,
                    (int)((MapManager.Camera.X - Game1.RenderWidth / 2) / MapManager.Camera.Scale),
                    (int)((MapManager.Camera.Y - Game1.RenderHeight / 2) / MapManager.Camera.Scale),
                    (int)(Game1.RenderWidth / MapManager.Camera.Scale),
                    (int)(Game1.RenderHeight / MapManager.Camera.Scale), AiComponent.Mask);
            }
            foreach (var gameObject in _objectList)
            {
                bool skipObject = (alwaysAnimateTypes == null) switch
                {
                    true  => (!gameObject.IsActive),
                    false => (!gameObject.IsActive || !ObjectManager.IsGameObjectType(gameObject, alwaysAnimateTypes))
                };
                if (!gameObject.IsActive || skipObject) { continue; }
                var aiComponent = gameObject.Components[AiComponent.Index] as AiComponent;
                if (aiComponent == null) { continue; }
                aiComponent?.CurrentState.Update?.Invoke();

                foreach (var trigger in aiComponent.CurrentState.Trigger)
                    trigger.Update();

                foreach (var trigger in aiComponent.Trigger)
                    trigger.Update();
            }
        }
    }
}
