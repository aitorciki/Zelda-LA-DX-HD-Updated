using System;
using System.IO;
using System.Globalization;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.CObjects;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.GameObjects.Things
{
    internal class ObjCrystal : GameObject
    {
        private readonly CBox _hittableBoxSmall;
        private readonly Color _lightColor;
        private readonly string _dialogPath;
        private readonly bool _isHardCrystal;

        bool light_source = true;
        int light_size = 80;

        int light_red_1 = 240;
        int light_grn_1 = 100;
        int light_blu_1 = 255;
        float light_bright_1 = 1.00f;

        int light_red_2 = 255;
        int light_grn_2 = 255;
        int light_blu_2 = 255;
        float light_bright_2 = 0.25f;

        public ObjCrystal(Map.Map map, int posX, int posY, string spriteId, int color, bool hardCrystal, string dialogPath) : base(map, spriteId)
        {
            // If a mod file exists load the values from it.
            string modFile = Path.Combine(Values.PathModFolder, "ObjCrystal.lahdmod");

            if (File.Exists(modFile))
                ModFile.Parse(modFile, this);

            var sprite = Resources.GetSprite(spriteId);

            EntityPosition = new CPosition(posX + 8, posY + 16, 0);
            EntitySize = new Rectangle(-40, -8 - 40, 80, 80);

            if (color == 0)
                _lightColor = new Color(light_red_1, light_grn_1, light_blu_1) * light_bright_1;
            else if (color == 1)
                _lightColor = new Color(light_red_2, light_grn_2, light_blu_2) * light_bright_2;

            _isHardCrystal = hardCrystal;
            _dialogPath = dialogPath;

            var box = new CBox(posX, posY + 16 - 12, 0, 16, 12, 16);
            var hittableBox = new CBox(EntityPosition, -7, -15, 0, 14, 13, 8);
            _hittableBoxSmall = new CBox(EntityPosition, -6, -13, 0, 12, 10, 8, true);

            if (!string.IsNullOrEmpty(_dialogPath))
                AddComponent(PushableComponent.Index, new PushableComponent(box, OnPush) { InertiaTime = 50 });
            AddComponent(CollisionComponent.Index, new BoxCollisionComponent(box, Values.CollisionTypes.Normal));
            AddComponent(HittableComponent.Index, new HittableComponent(hittableBox, OnHit));
            AddComponent(DrawComponent.Index, new DrawSpriteComponent(spriteId, EntityPosition, new Vector2(-8, -16), Values.LayerPlayer));
            AddComponent(LightDrawComponent.Index, new LightDrawComponent(DrawLight));
        }

        private bool OnPush(Vector2 direction, PushableComponent.PushType pushType)
        {
            if (pushType == PushableComponent.PushType.Impact)
                return false;

            // Don't show the "Oh? What a weird object!" message if disabled.
            if (!GameSettings.NoHelperText && _dialogPath == "crystal_hard")
                Game1.GameManager.StartDialogPath(_dialogPath);

            return false;
        }

        private void DrawLight(SpriteBatch spriteBatch)
        {
            if (light_source)
            {
                var _lightRectangle = new Rectangle((int)EntityPosition.X - light_size / 2, (int)EntityPosition.Y - 8 - light_size / 2, light_size, light_size);
                DrawHelper.DrawLight(spriteBatch, _lightRectangle, _lightColor);
            }
        }

        private Values.HitCollision OnHit(GameObject gameObject, Vector2 direction, HitType damageType, int damage, bool pieceOfPower)
        {
            if ((_isHardCrystal && damageType != HitType.PegasusBootsSword) || (damageType & HitType.SwordHold) != 0 || damageType == HitType.Hookshot)
                return Values.HitCollision.None;
            
            // this is really stupid
            // for the sword attacks a smaller hitbox is used
            if ((damageType & HitType.Sword) != 0 &&
                gameObject is ObjLink player && !player.IsPoking)
            {
                var collidingRec = player.SwordDamageBox.Rectangle().GetIntersection(_hittableBoxSmall.Box.Rectangle());
                var collidingArea = collidingRec.Width * collidingRec.Height;

                if (collidingArea < 16)
                    return Values.HitCollision.None;
            }

            Game1.GameManager.PlaySoundEffect("D378-09-09");

            // remove this object from the map
            Map.Objects.DeleteObjects.Add(this);

            // spawn small particle stones
            var mult = damageType == HitType.PegasusBootsSword ? 1.0f : 0.25f;
            var velZ = 0.5f;
            var diff = 200f;
            var vector0 = new Vector3(-1, -1, 0) * Game1.RandomNumber.Next(50, 75) / diff + new Vector3(direction * mult, velZ);
            var vector1 = new Vector3(-1, 0, 0) * Game1.RandomNumber.Next(50, 75) / diff + new Vector3(direction * mult, velZ);
            var vector2 = new Vector3(1, -1, 0) * Game1.RandomNumber.Next(50, 75) / diff + new Vector3(direction * mult, velZ);
            var vector3 = new Vector3(1, 0, 0) * Game1.RandomNumber.Next(50, 75) / diff + new Vector3(direction * mult, velZ);

            var stone0 = new ObjSmallStone(Map, (int)EntityPosition.X + 2, (int)EntityPosition.Y - 10, Game1.RandomNumber.Next(4, 8), vector0);
            var stone1 = new ObjSmallStone(Map, (int)EntityPosition.X + 2, (int)EntityPosition.Y - 6, Game1.RandomNumber.Next(4, 8), vector1);
            var stone2 = new ObjSmallStone(Map, (int)EntityPosition.X + 6, (int)EntityPosition.Y - 10, Game1.RandomNumber.Next(4, 8), vector2);
            var stone3 = new ObjSmallStone(Map, (int)EntityPosition.X + 6, (int)EntityPosition.Y - 6, Game1.RandomNumber.Next(4, 8), vector3);

            Map.Objects.SpawnObject(stone0);
            Map.Objects.SpawnObject(stone1);
            Map.Objects.SpawnObject(stone2);
            Map.Objects.SpawnObject(stone3);

            return Values.HitCollision.Blocking;
        }
    }
}