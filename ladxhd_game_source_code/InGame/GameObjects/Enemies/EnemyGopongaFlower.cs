using Microsoft.Xna.Framework;
using ProjectZ.InGame.GameObjects.Base;
using ProjectZ.InGame.GameObjects.Base.Components;
using ProjectZ.InGame.GameObjects.Base.CObjects;
using ProjectZ.InGame.GameObjects.Base.Components.AI;
using ProjectZ.InGame.SaveLoad;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.GameObjects.Enemies
{
    internal class EnemyGopongaFlower : GameObject
    {
        private readonly Animator _animator;
        private readonly DamageFieldComponent _damageField;
        private readonly AiDamageState _damageState;
        private readonly HittableComponent _hitComponent;
        private readonly BoxCollisionComponent _collisionComponent;

        private readonly int _animationLength;
        private bool _dealsDamage = true;
        private int _lives = ObjLives.GopongaFlower;

        private float _soundCooldown;
        private bool _blockSound = false;

        public EnemyGopongaFlower() : base("goponga flower") { }

        public EnemyGopongaFlower(Map.Map map, int posX, int posY) : base(map)
        {
            Tags = Values.GameObjectTag.Enemy;

            EntityPosition = new CPosition(posX + 8, posY + 8, 0);
            EntitySize = new Rectangle(-8, -8, 16, 16);

            _animator = AnimatorSaveLoad.LoadAnimator("Enemies/goponga flower");
            _animator.Play("idle");

            foreach (var frame in _animator.CurrentAnimation.Frames)
                _animationLength += frame.FrameTime;

            var sprite = new CSprite(EntityPosition);
            var animationComponent = new AnimationComponent(_animator, sprite, new Vector2(-8, -8));

            var body = new BodyComponent(EntityPosition, -8, -8, 16, 16, 8) { IgnoresZ = true };
            var collisionBox = new CBox(EntityPosition, -7, -7, 14, 14, 8);

            var hittableBox = new CBox(EntityPosition, -8, -8, 16, 16, 8);

            var aiComponent = new AiComponent();
            aiComponent.States.Add("idle", new AiState());
            _damageState = new AiDamageState(this, body, aiComponent, sprite, _lives)
            {
                HitMultiplierX = 0,
                HitMultiplierY = 0,
                OnBurn = OnBurn
            };
            aiComponent.ChangeState("idle");

            AddComponent(AiComponent.Index, aiComponent);
            AddComponent(DamageFieldComponent.Index, _damageField = new DamageFieldComponent(hittableBox, HitType.Enemy, 4));
            AddComponent(CollisionComponent.Index, _collisionComponent = new BoxCollisionComponent(collisionBox, Values.CollisionTypes.Enemy));
            AddComponent(HittableComponent.Index, _hitComponent = new HittableComponent(hittableBox, OnHit));
            AddComponent(BodyComponent.Index, body);
            AddComponent(BaseAnimationComponent.Index, animationComponent);
            AddComponent(UpdateComponent.Index, new UpdateComponent(Update));
            AddComponent(DrawComponent.Index, new BodyDrawComponent(body, sprite, Values.LayerPlayer) { WaterOutline = false });
        }

        private void Update()
        {
            if (_blockSound)
            {
                _soundCooldown += Game1.DeltaTime;
                if (_soundCooldown > 220) 
                {
                    _soundCooldown = 0;
                    _blockSound = false;
                }
            }
            // @HACK: this is used to sync all the animations with the same length
            // otherwise they would not be in sync if they did not get updated at the same time
            _animator.SetFrame(0);
            _animator.SetTime(Game1.TotalGameTime % _animationLength);
            _animator.Update();
        }

        private void OnBurn()
        {
            _animator.Pause();
            _dealsDamage = false;
            _damageField.IsActive = false;
            RemoveComponent(CollisionComponent.Index);
        }

        private bool ValidateHit(HitType hitType, bool pieceOfPower)
        {
            // What can kill these:
            // Bow-wow ; Hookshot ; Magic Rod ; Boomerang ; Sword2 + Spin Slash ; Sword2 + Piece of Power/Red Tunic
            if (hitType == HitType.BowWow || hitType == HitType.Hookshot || hitType == HitType.MagicRod ||  hitType == HitType.Boomerang ||
                ((hitType & HitType.Sword2) != 0 && (hitType & HitType.SwordSpin) != 0) ||  
                ((hitType & HitType.Sword2) != 0 && pieceOfPower))
            {
                return true;
            }
            return false;
        }

        private Values.HitCollision OnHit(GameObject originObject, Vector2 direction, HitType type, int damage, bool pieceOfPower)
        {
            if (ValidateHit(type, pieceOfPower))
            {
                if (type != HitType.BowWow && (type == HitType.MagicRod || damage >= _damageState.CurrentLives))
                {
                    _damageState.HitMultiplierX = 4;
                    _damageState.HitMultiplierY = 4;
                }
                if (_dealsDamage)
                {
                    if (_damageState.CurrentLives <= 0)
                    {
                        _damageField.IsActive = false;
                        _hitComponent.IsActive = false;
                        _collisionComponent.IsActive = false;
                    }
                    return _damageState.OnHit(originObject, direction, type, damage, pieceOfPower);
                }
                return Values.HitCollision.None;
            }
            if (!_blockSound)
            {
                Game1.GameManager.PlaySoundEffect("D360-09-09");
                _blockSound = true;
            }
            return Values.HitCollision.Blocking;
        }
    }
}