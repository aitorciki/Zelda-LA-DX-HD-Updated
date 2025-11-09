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
            var Link = MapManager.ObjLink;
            _objectList.Clear();

            // Classic Camera: Only update objects within the current field.
            if (Camera.ClassicMode)
            {
                Pool.GetComponentList(_objectList, 
                    ObjectManager.UpdateField.X, 
                    ObjectManager.UpdateField.Y,
                    ObjectManager.UpdateField.Width, 
                    ObjectManager.UpdateField.Height, AiComponent.Mask);
                _objectList.RemoveAll(o => o?.EntityPosition != null && !ObjectManager.ActualField.Contains(o.EntityPosition.Position));
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
            // Always include Link's follower, the boomerang, and BowWow (when rescued).
            foreach (var gameObject in new GameObject?[] { Link._objFollower, Link.Boomerang, Link._objBowWow })
            {
                if (gameObject != null && !_objectList.Contains(gameObject))
                    _objectList.Add(gameObject);
            }
            // Always include certain objects that are flagged as "always animate".
            foreach (var gameObject in ObjectManager.AlwaysAnimateObjectsTemp)
            {
                if (gameObject != null && !gameObject.IsDead && !_objectList.Contains(gameObject))
                    _objectList.Add(gameObject);
            }
            // Update all game object AI components in the list.
            foreach (var gameObject in _objectList)
            {
                bool skipObject = freezePersistTypes == null
                    ? !gameObject.IsActive
                    : !gameObject.IsActive || !ObjectManager.IsGameObjectType(gameObject, freezePersistTypes);

                if (skipObject) { continue; }

                if (gameObject.Components[AiComponent.Index] is AiComponent aiComponent)
                {
                    aiComponent.CurrentState.Update?.Invoke();
                    foreach (var trigger in aiComponent.CurrentState.Trigger)
                        trigger.Update();
                    foreach (var trigger in aiComponent.Trigger)
                        trigger.Update();
                }
            }
        }

    }
}
