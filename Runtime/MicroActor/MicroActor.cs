using Pixygon.DebugTool;
using Pixygon.Effects;
using UnityEngine;

namespace Pixygon.Micro {
    public class MicroActor : MonoBehaviour {
        [SerializeField] protected SpriteRenderer _sprite;
        [SerializeField] protected Animator _anim;
        [SerializeField] private bool _destroyOnDeath;
        [SerializeField] private Rigidbody2D _rigid;
        [SerializeField] private MovementConfig _movementConfig;
        [SerializeField] private GroundChecker _groundChecker;
        private float _killheight;
        protected LevelLoader _levelLoader;
        protected IFrameManager _iFrameManager;

        public Animator Anim => _anim;
        public SpriteRenderer Renderer => _sprite;
        public bool Invincible { get; set; }
        public MicroActorData Data { get; private set; }
        public bool IsDead { get; protected set; }
        public bool IgnoreMovement { get; protected set; }
        public int HP { get; protected set;}
        public Rigidbody2D Rigid => _rigid;
        public MovementConfig MovementConfig => _movementConfig;
        public GroundChecker GroundChecker => _groundChecker;

        public virtual void Initialize(LevelLoader loader, MicroActorData data) {
            Log.DebugMessage(DebugGroup.PixygonMicro, "Initializing MicroActor");
            Data = data;
            HP = data._hp;
            if (!Data._isKillable)
                Invincible = true;
            if (Data._isHostile)
                gameObject.AddComponent<DamageObject>();
            _levelLoader = loader;
            _killheight = _levelLoader.CurrentLevel.KillHeight;
            if (Data._useIframes) {
                _iFrameManager = gameObject.AddComponent<IFrameManager>();
                _iFrameManager.Initialize(this, _sprite);
            }
        }

        public virtual void Damage() {
            if (Invincible) return;
            if (!Data._isKillable) return;
            if(Data._useIframes)
                _iFrameManager.SetIFrames();
            if(Data._damageFx != null)
                EffectsManager.SpawnEffect(Data._damageFx.GetFullID, transform.position);
            //_anim.Damage();
            if (HP != 0)
                HP -= 1;
            else
                Die();
        }

        protected virtual void Die() {
            IsDead = true;
            if(Data._useIframes)
                _iFrameManager.StopIFrames();
            _sprite.enabled = false;
            if(Data._deathFx != null)
                EffectsManager.SpawnEffect(Data._deathFx.GetFullID, transform.position);
            if(_destroyOnDeath) Destroy(gameObject);
        }

        public virtual void Update() {
            if (Data == null) return;
            if (transform.position.y <= _killheight)
                Die();
            if(Data._isKillable && Data._useIframes)
                _iFrameManager.HandleIFrames();
        }
        
        public void SetFlip(float dir) {
            if (dir < -0.1f && !Renderer.flipX)
                Renderer.flipX = true;
            else if (dir > 0.1f && Renderer.flipX)
                Renderer.flipX = false;
        }
    }
}
