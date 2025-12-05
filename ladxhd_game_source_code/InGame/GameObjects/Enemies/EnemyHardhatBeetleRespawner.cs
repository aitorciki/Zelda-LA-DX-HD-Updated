using Microsoft.Xna.Framework;
using ProjectZ.Base;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.CObjects;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.Map;
using ProjectZ.InGame.GameObjects.Enemies;

namespace ProjectZ.InGame.GameObjects.Things
{
    internal class EnemyHardhatBeetleRespawner : GameObject
    {
        private RectangleF _field;
        private bool _respawnStart;
        private float _respawnTimer;

        public EnemyHardhatBeetleRespawner(Map.Map map, int posX, int posY, RectangleF field)
            : base(map)
        {
            EntityPosition = new CPosition(posX, posY, 0);
            EntitySize = new Rectangle(0, 0, 16, 16);

            _field = field;

            AddComponent(UpdateComponent.Index, new UpdateComponent(Update));
            Map.Objects.RegisterAlwaysAnimateObject(this);
        }

        private void Update()
        {
            // Classic Mode: respawn after field transition
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
                        _respawnStart = false;
                        _respawnTimer = 0;
                    }
                }
            }
            // Standard camera: Respawn when player leaves the field.
            else
            {
                if (!_field.Contains(MapManager.ObjLink.EntityPosition.Position))
                    SpawnBeetle();
            }
        }

        private void SpawnBeetle()
        {
            // Remove the respawner itself
            Map.Objects.DeleteObjects.Add(this);

            // Spawn the original enemy
            Map.Objects.SpawnObject(new EnemyHardhatBeetle(Map, (int)EntityPosition.X, (int)EntityPosition.Y));
        }
    }
}
