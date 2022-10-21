using UnityEngine;

namespace Pixygon.Micro {
    public class MicroActor : MonoBehaviour {
        [SerializeField] protected SpriteRenderer _sprite;
        [SerializeField] private ParticleSystem _damageFx;
        [SerializeField] private ParticleSystem _deathFx;
        [SerializeField] private AudioSource _damageSfx;
        [SerializeField] private AnimatorController _anim;
        [SerializeField] protected int _startHp;
        [SerializeField] private float _threshold = -8f;
        [SerializeField] private bool _destroyOnDeath;

        protected int _hp;
        private float _iFrames;
        private float _iFrameEffectCounter;
        protected bool _invincible;
        public bool IsDead { get; protected set; }

        public virtual void Initialize(LevelLoader loader) {
            _hp = _startHp;
        }
        protected void HandleIFrames() {
            if (!_invincible) return;
            if (_iFrames > 0f) {
                _iFrames -= Time.deltaTime;
                if (_iFrameEffectCounter > 0f)
                    _iFrameEffectCounter -= Time.deltaTime;
                else {
                    _iFrameEffectCounter = .1f;
                    _sprite.enabled = !_sprite.enabled;
                }
            }
            else
                StopIFrames();
        }

        private void StopIFrames() {
            _iFrameEffectCounter = 0f;
            _invincible = false;
            _sprite.enabled = true;
        }

        public virtual void Damage() {
            if (_invincible) return;
            _iFrames = 1f;
            _invincible = true;
            _damageFx.Play();
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
            if (transform.position.y <= _threshold)
                Die();
            HandleIFrames();
        }
    }
}