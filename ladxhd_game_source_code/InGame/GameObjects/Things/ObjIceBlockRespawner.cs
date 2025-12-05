using Microsoft.Xna.Framework;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.CObjects;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.Map;

namespace ProjectZ.InGame.GameObjects.Things
{
    internal class ObjIceBlockRespawner : GameObject
    {
        private int _lastFieldTime;
        private bool _respawnStart;
        private float _respawnTimer;
        private readonly bool _respawnedFromSpawner;

        public ObjIceBlockRespawner(Map.Map map, int posX, int posY, bool fromSpawner) : base(map)
        {
            EntityPosition = new CPosition(posX, posY, 0);
            EntitySize = new Rectangle(0, 0, 16, 16);

            _respawnedFromSpawner = fromSpawner;
            _lastFieldTime = Map.GetUpdateState(EntityPosition.Position);

            AddComponent(UpdateComponent.Index, new UpdateComponent(Update));
            Map.Objects.RegisterAlwaysAnimateObject(this);
        }

        private void Update()
        {
            // If this respawner already spawned the block this frame, delete it
            if (_respawnedFromSpawner)
                DeleteThis();

            if (Camera.ClassicMode)
            {
                // Classic mode = respawn when changing fields
                if (MapManager.ObjLink.FieldChange)
                    _respawnStart = true;

                if (_respawnStart)
                {
                    _respawnTimer += Game1.DeltaTime;
                    if (_respawnTimer >= 250)
                    {
                        SpawnIceBlock();
                        _respawnStart = false;
                        _respawnTimer = 0;
                    }
                }
            }
            else
            {
                // Standard camera = respawn when player re-enters the field after leaving
                var updateState = Map.GetUpdateState(EntityPosition.Position);

                if (_lastFieldTime < updateState)
                    SpawnIceBlock();
            }
        }

        private void DeleteThis()
        {
            Map.Objects.DeleteObjects.Add(this);
        }

        private void SpawnIceBlock()
        {
            // Remove respawner
            Map.Objects.DeleteObjects.Add(this);

            // Respawn Ice Block
            Map.Objects.SpawnObject(new ObjIceBlock(Map,
                (int)EntityPosition.X,
                (int)EntityPosition.Y));
        }
    }
}
