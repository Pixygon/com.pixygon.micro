using Pixygon.Actors;
using Pixygon.Core;
using Pixygon.DebugTool;
using Pixygon.Effects;
using UnityEngine;
using UnityEngine.Events;

namespace Pixygon.Micro {
    public class MicroActor : Actors.Actor {
        [SerializeField] protected SpriteRenderer _sprite; //Added
        [SerializeField] protected Animator _anim;
        [SerializeField] private bool _destroyOnDeath;
        [SerializeField] private Rigidbody2D _rigid;
        [SerializeField] private MovementConfig _movementConfig;
        [SerializeField] private GroundChecker _groundChecker;
        private float _killheight;
        protected LevelLoader _levelLoader;
        protected IFrameHandler _iFrameManager; //Added
        private bool _isAnimNotNull;
        protected float _defaultAnimSpeed = 1f;

        protected UnityEvent _actorOnKill;

        public Animator Anim => _anim;
        public SpriteRenderer Renderer => _sprite;
        public bool Invincible { get; set; }
        public bool IgnoreMovement { get; protected set; }
        public int HP { get; protected set;}
        public Rigidbody2D Rigid => _rigid;
        public MovementConfig MovementConfig => _movementConfig;
        public GroundChecker GroundChecker => _groundChecker;
        public Vector3 LastSafePosition { get; set; }

        private void OnEnable() {   //Added
            PauseManager.OnPause += OnPause;
            PauseManager.OnUnpause += OnUnpause;
        }

        private void OnDisable() {   //Added
            PauseManager.OnPause -= OnPause;
            PauseManager.OnUnpause -= OnUnpause;
        }

        public virtual void Initialize(LevelLoader loader, ActorData data, UnityEvent _events = null) {
            Log.DebugMessage(DebugGroup.PixygonMicro, "Initializing MicroActor");
            ActorData = data;
            HP = data._hp;
            if (!ActorData._isKillable)
                Invincible = true;
            if (ActorData._isHostile)
                gameObject.AddComponent<DamageObject>();
            _levelLoader = loader;
            _killheight = _levelLoader.CurrentLevel.KillHeight;
            if (ActorData._useIframes) {
                _iFrameManager = gameObject.AddComponent<IFrameHandler>();
                _iFrameManager.Initialize(this, _sprite);
            }
            _isAnimNotNull = _anim != null;
            if(_isAnimNotNull) _anim.speed = _defaultAnimSpeed;
            if(_events != null)
                _actorOnKill = _events;
        }
        //Added
        protected virtual void OnPause() {
            _isPaused = true;
            if(_rigid != null) _rigid.Sleep();
            if (_anim == null) {
                Debug.Log("Hey! I've been... removed??");
                Debug.Log(gameObject.name);
            }
            if(_isAnimNotNull && _anim != null) _anim.speed = 0f;
        }
        //Added
        protected virtual void OnUnpause() {
            _isPaused = false;
            if(_rigid != null) _rigid.WakeUp();
            if(_isAnimNotNull && _anim != null) _anim.speed = _defaultAnimSpeed;
        }
        public virtual void Damage(bool resetPosition = false) {
            if (IsDead) return;
            if (Invincible) return;
            if (!ActorData._isKillable) return;
            if(ActorData._useIframes)
                _iFrameManager.SetIFrames();
            if(ActorData._damageFx != null)
                EffectsManager.SpawnEffect(ActorData._damageFx.GetFullID, transform.position);
            //_anim.Damage();
            if (HP != 0)
                HP -= 1;
            else
                Die();
        }

        protected virtual void Die() {
            IsDead = true;
            if(ActorData._useIframes)
                _iFrameManager.StopIFrames();
            _sprite.enabled = false;
            if(ActorData._deathFx != null)
                EffectsManager.SpawnEffect(ActorData._deathFx.GetFullID, transform.position);
            Debug.Log("Hello i died???");
            _actorOnKill?.Invoke();
            if(_destroyOnDeath) Destroy(gameObject);
            Debug.Log("Hello i died!");
        }

        public void InstaKill() {
            Die();
        }

        public virtual void Update() {
            if (ActorData == null) return;
            if (_isPaused) return;
            if (transform.position.y <= _killheight)
                Die();
            if(ActorData._isKillable && ActorData._useIframes)
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
