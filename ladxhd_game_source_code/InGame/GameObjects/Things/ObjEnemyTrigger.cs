using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.CObjects;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.GameObjects.Things
{
    internal class ObjEnemyTrigger : GameObject
    {
        public List<GameObject> EnemyTriggerList = new List<GameObject>();
        private readonly Rectangle _triggerField;
        private readonly string _triggerKey;

        private int _posX;
        private int _posY;
        private bool _enemiesAlive;
        private bool _init;
        private bool _respawn;

        public ObjEnemyTrigger() : base("editor enemy trigger") { }

        public ObjEnemyTrigger(Map.Map map, int posX, int posY, string triggerKey, bool respawn) : base(map)
        {
            EntityPosition = new CPosition(posX, posY, 0);

            Tags = Values.GameObjectTag.Utility;

            if (string.IsNullOrEmpty(triggerKey))
            {
                IsDead = true;
                return;
            }
            _posX = posX;
            _posY = posY;
            _triggerKey = triggerKey;
            _triggerField = map.GetField(posX, posY);
            _respawn = respawn;

            AddComponent(UpdateComponent.Index, new UpdateComponent(Update));
        }

        private void Update()
        {
            // this gets called the first time update is run so it can capture enemies spawned by an ObjObjectSpawner
            if (!_init)
            {
                _init = true;

                // get the enemies the object should watch over
                Map.Objects.GetGameObjectsWithTag(EnemyTriggerList, Values.GameObjectTag.Enemy,
                    _triggerField.X, _triggerField.Y, _triggerField.Width, _triggerField.Height);
            }
            _enemiesAlive = false;

            // check if the enemies where deleted from the map
            foreach (var gameObject in EnemyTriggerList)
                if (gameObject.Map != null)
                    _enemiesAlive = true;

            if (_enemiesAlive)
                return;

            Game1.GameManager.SaveManager.SetString(_triggerKey, "1");

            //RemoveComponent(UpdateComponent.Index);
            // remove the object
            Map.Objects.DeleteObjects.Add(this);

            if (_respawn)
                Map.Objects.SpawnObject(new ObjEnemyTrigger(Map, _posX, _posY, _triggerKey, _respawn));
        }
    }
}