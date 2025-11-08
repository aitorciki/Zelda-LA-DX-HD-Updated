using System;
using Microsoft.Xna.Framework;
using ProjectZ.Base;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.CObjects;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.GameObjects.Base.Components.AI;
using ProjectZ.InGame.GameObjects.Things;
using ProjectZ.InGame.Map;
using ProjectZ.InGame.SaveLoad;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.GameObjects.Enemies
{
    internal class EnemyRaven : GameObject
    {
        private readonly BodyComponent _body;
        private readonly AiComponent _aiComponent;
        private readonly Animator _animator;
        private readonly AiDamageState _damageState;
        private readonly AiTriggerTimer _followTimer;
        private readonly HittableComponent _hitComponent;
        private readonly PushableComponent _pushComponent;
        private readonly DamageFieldComponent _damageField;
        private CSprite _sprite;
        private readonly Box _activationBox;

        private float _flapCounter;
        private float _fadeOutTime = 750;
        private double _dirRadius;
        private int _dirIndex;
        private int _lives = ObjLives.Raven;

        public EnemyRaven() : base("raven") { }

        public EnemyRaven(Map.Map map, int posX, int posY) : base(map)
        {
            Tags = Values.GameObjectTag.Enemy;

            EntityPosition = new CPosition(posX + 8, posY + 12, 0);
            EntitySize = new Rectangle(-8, -32, 16, 32);

            _activationBox = new Box(posX + 8 - 20, posY - 32, 0, 40, 90, 16);

            _animator = AnimatorSaveLoad.LoadAnimator("Enemies/raven");

            _sprite = new CSprite(EntityPosition);
            var animationComponent = new AnimationComponent(_animator, _sprite, new Vector2(-7, -16));

            _body = new BodyComponent(EntityPosition, -6, -14, 12, 14, 8)
            {
                CollisionTypes = Values.CollisionTypes.None,
                IgnoreHoles = true,
                IgnoresZ = true
            };

            var stateWaiting = new AiState(UpdateWaiting);
            var stateStart = new AiState(UpdateStart);
            var stateFlying = new AiState(UpdateFlying);
            stateFlying.Trigger.Add(_followTimer = new AiTriggerTimer(1000));

            _aiComponent = new AiComponent();
            _aiComponent.States.Add("waiting", stateWaiting);
            _aiComponent.States.Add("start", stateStart);
            _aiComponent.States.Add("flying", stateFlying);
            _damageState = new AiDamageState(this, _body, _aiComponent, _sprite, _lives, true, false);

            _aiComponent.ChangeState("waiting");

            // the player can jump over the enemy...
            var damageCollider = new CBox(EntityPosition, -6, -14, 0, 12, 14, 8, true);

            AddComponent(DamageFieldComponent.Index, _damageField = new DamageFieldComponent(damageCollider, HitType.Enemy, 2));
            AddComponent(HittableComponent.Index, _hitComponent = new HittableComponent(damageCollider, OnHit));
            AddComponent(PushableComponent.Index, _pushComponent = new PushableComponent(damageCollider, OnPush));
            AddComponent(BodyComponent.Index, _body);
            AddComponent(AiComponent.Index, _aiComponent);
            AddComponent(BaseAnimationComponent.Index, animationComponent);
            AddComponent(DrawComponent.Index, new BodyDrawComponent(_body, _sprite, Values.LayerTop));
            AddComponent(DrawShadowComponent.Index, new BodyDrawShadowComponent(_body, _sprite));

            new ObjSpriteShadow("sprshadowm", this, Values.LayerPlayer, map);
            ObjectManager.AlwaysAnimateObjectsMain.Add(this);
        }

        private void UpdateWaiting()
        {
            _dirIndex = MapManager.ObjLink.PosX < EntityPosition.X ? 0 : 1;

            _animator.Play("idle_" + _dirIndex);

            // activate the crow
            if (MapManager.ObjLink._body.BodyBox.Box.Intersects(_activationBox))
            {
                _aiComponent.ChangeState("start");
            }
        }

        private void UpdateFlyingSound()
        {
            _flapCounter += Game1.DeltaTime;

            if (_lives > 0 && _flapCounter > 430)
            {
                Game1.GameManager.PlaySoundEffect("D378-45-2D");
                _flapCounter = 0;
            }
        }

        private void UpdateStart()
        {
            _animator.Play("fly_" + _dirIndex);

            EntityPosition.Set(new Vector3(
                EntityPosition.X,
                EntityPosition.Y,
                EntityPosition.Z + 0.5f * Game1.TimeMultiplier));

            if (EntityPosition.Z >= 15)
            {
                EntityPosition.Z = 15;
                _aiComponent.ChangeState("flying");
                _dirRadius = Math.Atan2(MapManager.ObjLink.PosY - EntityPosition.Y, MapManager.ObjLink.PosX - EntityPosition.X);
            }
            UpdateFlyingSound();
        }

        private void UpdateFlying()
        {
            var direction = MapManager.ObjLink.EntityPosition.Position - new Vector2(EntityPosition.X, EntityPosition.Y - EntityPosition.Z);
            var directionRadius = Math.Atan2(direction.Y, direction.X);
            var distance = direction.Length();

            if (distance < 80)
            {
                var followSpeed = 0.02f;
                if (directionRadius < _dirRadius - followSpeed || _followTimer.State)
                    _dirRadius -= followSpeed * Game1.TimeMultiplier;
                else if (directionRadius > _dirRadius + followSpeed)
                    _dirRadius += followSpeed * Game1.TimeMultiplier;
            }

            var velocity = new Vector2((float)Math.Cos(_dirRadius), (float)Math.Sin(_dirRadius));
            _body.VelocityTarget = velocity * 1.25f;

            _dirIndex = velocity.X < 0 ? 0 : 1;
            _animator.Play("fly_" + _dirIndex);

            if (distance < 85)
                UpdateFlyingSound();

            if (distance > 100)
                FadeOutDelete();
        }

        private bool OnPush(Vector2 direction, PushableComponent.PushType type)
        {
            if (type == PushableComponent.PushType.Impact)
                _body.Velocity = new Vector3(direction * 1.75f, _body.Velocity.Z);

            return true;
        }

        private void FadeOutDelete()
        {
            _fadeOutTime -= Game1.DeltaTime;

            if (_fadeOutTime < 0)
                Map.Objects.DeleteObjects.Add(this);
            else
                _sprite.Color = Color.White * MathHelper.Clamp(_fadeOutTime / 100, 0, 1);
        }

        private Values.HitCollision OnHit(GameObject gameObject, Vector2 direction, HitType damageType, int damage, bool pieceOfPower)
        {
            if (damageType == HitType.MagicPowder)
                return Values.HitCollision.None;

            if (damageType == HitType.Bow || damageType == HitType.MagicRod)
                damage /= 2;

            // start attacking?
            if (_aiComponent.CurrentStateId == "waiting" && (damageType == HitType.Bomb || damageType == HitType.ThrownObject))
            {
                _aiComponent.ChangeState("start");

                return Values.HitCollision.None;
            }
            if (_damageState.CurrentLives <= 0)
            {
                _damageField.IsActive = false;
                _hitComponent.IsActive = false;
                _pushComponent.IsActive = false;
            }
            return _damageState.OnHit(gameObject, direction, damageType, damage, pieceOfPower);
        }
    }
}