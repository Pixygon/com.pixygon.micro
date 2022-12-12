using Pixygon.DebugTool;
using UnityEngine;

namespace Pixygon.Micro {
    public class MicroActor : MonoBehaviour {
        [SerializeField] protected SpriteRenderer _sprite;
        [SerializeField] private ParticleSystem _damageFx;
        [SerializeField] protected ParticleSystem _deathFx;
        [SerializeField] private AudioSource _damageSfx;
        [SerializeField] private AnimatorController _anim;
        [SerializeField] private bool _destroyOnDeath;
        [SerializeField] private float _iFrameLength = .6f;

        private float _iFrames;
        private float _iFrameEffectCounter;
        protected LevelLoader _levelLoader;
        private float _killheight;
        private bool _iFrameRed;
        
        public bool Invincible { get; private set; }
        public MicroActorData Data { get; private set; }
        public bool IsDead { get; protected set; }
        public bool IgnoreMovement { get; protected set; }
        public int HP { get; protected set;}

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
        }

        protected void HandleIFrames() {
            if (!Invincible) return;
            if (_iFrames > 0f) {
                _iFrames -= Time.deltaTime;
                if (_iFrameEffectCounter > 0f)
                    _iFrameEffectCounter -= Time.deltaTime;
                else {
                    _iFrameRed = !_iFrameRed;
                    _iFrameEffectCounter = .1f;
                    _sprite.color = _iFrameRed ? Color.red : Color.white;
                }
            }
            else
                StopIFrames();
        }

        protected void StopIFrames() {
            _iFrameEffectCounter = 0f;
            Invincible = false;
            _iFrameRed = false;
            _sprite.color = Color.white;
        }

        public virtual void Damage() {
            if (Invincible) return;
            if (!Data._isKillable) return;
            _iFrames = _iFrameLength;
            Invincible = true;
            _damageFx.Play();
            _damageSfx.pitch = UnityEngine.Random.Range(0.9f, 1.05f);
            _damageSfx.Play();
            _anim.Damage();
            if (HP != 0)
                HP -= 1;
            else
                Die();
        }

        protected virtual void Die() {
            IsDead = true;
            StopIFrames();
            _sprite.enabled = false;
            _deathFx.Play();
            if(_destroyOnDeath)
                Destroy(gameObject);
        }

        public virtual void Update() {
            if (Data == null) return;
            if (transform.position.y <= _killheight)
                Die();
            if(Data._isKillable)
                HandleIFrames();
        }
    }
}