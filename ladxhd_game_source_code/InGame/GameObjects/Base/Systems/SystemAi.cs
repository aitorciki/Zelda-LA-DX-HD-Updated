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

            // Classic Camera: Only update objects within the current field.
            if (Camera.ClassicMode)
            {
                Pool.GetComponentList(_objectList, ObjectManager.UpdateField.X, ObjectManager.UpdateField.Y, 
                    ObjectManager.UpdateField.Width, ObjectManager.UpdateField.Height, AiComponent.Mask);
                _objectList.RemoveAll(o => o.EntityPosition != null && !ObjectManager.ActualField.Contains(o.EntityPosition.Position));
            }
            // Normal Camera: Update objects that are within the viewport.
            else
            {
                Pool.GetComponentList(_objectList,
                    (int)((MapManager.Camera.X - Game1.RenderWidth / 2) / MapManager.Camera.Scale),
                    (int)((MapManager.Camera.Y - Game1.RenderHeight / 2) / MapManager.Camera.Scale),
                    (int)(Game1.RenderWidth / MapManager.Camera.Scale),
                    (int)(Game1.RenderHeight / MapManager.Camera.Scale), AiComponent.Mask);
            }
            // Always update Link's follower, the boomerang, and BowWow (when rescued).
            if (!_objectList.Contains(MapManager.ObjLink._objFollower) && MapManager.ObjLink._objFollower != null)
                _objectList.Add(MapManager.ObjLink._objFollower);
            if (!_objectList.Contains(MapManager.ObjLink.Boomerang) && MapManager.ObjLink.Boomerang != null)
                _objectList.Add(MapManager.ObjLink.Boomerang);
            if (!_objectList.Contains(MapManager.ObjLink._objBowWow) && MapManager.ObjLink._objBowWow != null)
                _objectList.Add(MapManager.ObjLink._objBowWow);

            // Always update certain objects that are flagged as "always animate".
            foreach (var updObject in ObjectManager.AlwaysAnimateObjectsTemp)
            {
                if (!_objectList.Contains(updObject) && !updObject.IsDead && updObject != null)
                    _objectList.Add(updObject);
            }
            // Update all game object AI components in the list.
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
