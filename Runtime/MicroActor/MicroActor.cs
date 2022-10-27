using Pixygon.DebugTool;
using UnityEngine;

namespace Pixygon.Micro {
    public class MicroActor : MonoBehaviour {
        [SerializeField] protected SpriteRenderer _sprite;
        [SerializeField] private ParticleSystem _damageFx;
        [SerializeField] private ParticleSystem _deathFx;
        [SerializeField] private AudioSource _damageSfx;
        [SerializeField] private AnimatorController _anim;
        [SerializeField] private float _threshold = -8f;
        [SerializeField] private bool _destroyOnDeath;
        [SerializeField] private float _iFrameLength = .6f;

        protected int _hp;
        private float _iFrames;
        private float _iFrameEffectCounter;
        public bool Invincible { get; private set; }
        public MicroActorData Data { get; private set; }
        public bool IsDead { get; protected set; }

        public virtual void Initialize(LevelLoader loader, MicroActorData data) {
            Log.DebugMessage(DebugGroup.PixygonMicro, "Initializing MicroActor");
            Data = data;
            _hp = data._hp;
            if (!Data._isKillable)
                Invincible = true;
            if (Data._isHostile)
                gameObject.AddComponent<DamageObject>();
        }

        private bool _iFrameRed;
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

        private void StopIFrames() {
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
            if (_hp != 0)
                _hp -= 1;
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
            if (transform.position.y <= _threshold)
                Die();
            if(Data._isKillable)
                HandleIFrames();
        }
    }
}