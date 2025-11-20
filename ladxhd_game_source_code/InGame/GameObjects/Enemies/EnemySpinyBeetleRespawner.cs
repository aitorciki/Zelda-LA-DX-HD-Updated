using Microsoft.Xna.Framework;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.CObjects;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.GameObjects.Enemies;
using ProjectZ.InGame.Map;

namespace ProjectZ.InGame.GameObjects.Things
{
    internal class EnemySpinyBeetleRespawner : GameObject
    {
        private readonly int _type;       // 0=grass, 1=stone, 2=skull
        private readonly int _posX;
        private readonly int _posY;

        private int _lastFieldTime;
        private bool _respawnStart;
        private float _respawnTimer;

        public EnemySpinyBeetleRespawner(Map.Map map, int posX, int posY, int type) : base(map)
        {
            EntityPosition = new CPosition(posX, posY, 0);
            EntitySize = new Rectangle(0, 0, 16, 16);

            _posX = posX;
            _posY = posY;
            _type = type;

            _lastFieldTime = Map.GetUpdateState(EntityPosition.Position);

            AddComponent(UpdateComponent.Index, new UpdateComponent(Update));
            Map.Objects.RegisterAlwaysAnimateObject(this);
        }

        private void Update()
        {
            // Classic camera mode – respawn after field change
            if (Camera.ClassicMode)
            {
                if (MapManager.ObjLink.FieldChange)
                    _respawnStart = true;

                if (_respawnStart)
                {
                    _respawnTimer += Game1.DeltaTime;
                    if (_respawnTimer >= 250)
                    {
                        SpawnBeetle();
                        _respawnTimer = 0;
                        _respawnStart = false;
                    }
                }
            }
            // Otherwise respawn when sufficiently far away
            else
            {
                var updateState = Map.GetUpdateState(EntityPosition.Position);
                if (_lastFieldTime < updateState)
                    SpawnBeetle();
            }
        }

        private void SpawnBeetle()
        {
            Map.Objects.DeleteObjects.Add(this);
            Map.Objects.SpawnObject(new EnemySpinyBeetle(Map, _posX, _posY, _type));
        }
    }
}
