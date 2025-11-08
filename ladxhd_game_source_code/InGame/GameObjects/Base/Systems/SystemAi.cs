using System;
using System.Collections.Generic;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.GameObjects.Base.Pools;
using ProjectZ.InGame.Map;

namespace ProjectZ.InGame.GameObjects.Base.Systems
{
    class SystemAi
    {
        public ComponentPool Pool;

        private readonly List<GameObject> _objectList = new List<GameObject>();

        public void Update(Type[] freezePersistTypes = null)
        {
            _objectList.Clear();

            // Only update objects that are within the current field.
            if (Camera.ClassicMode)
            {
                Pool.GetComponentList(_objectList, ObjectManager.UpdateField.X, ObjectManager.UpdateField.Y, 
                    ObjectManager.UpdateField.Width, ObjectManager.UpdateField.Height, AiComponent.Mask);
                _objectList.RemoveAll(o => o.EntityPosition != null && !ObjectManager.ActualField.Contains(o.EntityPosition.Position));

                // Always update Link's follower, the boomerang, and BowWow (when rescuing him).
                if (!_objectList.Contains(MapManager.ObjLink._objFollower) && MapManager.ObjLink._objFollower != null)
                    _objectList.Add(MapManager.ObjLink._objFollower);
                if (!_objectList.Contains(MapManager.ObjLink.Boomerang) && MapManager.ObjLink.Boomerang != null)
                    _objectList.Add(MapManager.ObjLink.Boomerang);
                if (!_objectList.Contains(MapManager.ObjLink._objBowWow) && MapManager.ObjLink._objBowWow != null)
                    _objectList.Add(MapManager.ObjLink._objBowWow);

                foreach (var updObject in ObjectManager.AlwaysAnimateObjectsTemp)
                {
                    if (!_objectList.Contains(updObject) && !updObject.IsDead && updObject != null)
                        _objectList.Add(updObject);
                }
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
                bool skipObject = (freezePersistTypes == null) switch
                {
                    true  => (!gameObject.IsActive),
                    false => (!gameObject.IsActive || !ObjectManager.IsGameObjectType(gameObject, freezePersistTypes))
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
