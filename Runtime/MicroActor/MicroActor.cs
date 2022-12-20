using Pixygon.DebugTool;
using Pixygon.Effects;
using UnityEngine;

namespace Pixygon.Micro {
    public class MicroActor : MonoBehaviour {
        [SerializeField] protected SpriteRenderer _sprite;
        [SerializeField] private EffectData _damageFx;
        [SerializeField] private EffectData _deathFx;
        [SerializeField] private AnimatorController _anim;
        [SerializeField] private bool _destroyOnDeath;
        
        protected LevelLoader _levelLoader;
        private float _killheight;
        private IFrameManager _iFrameManager;
        
        public bool Invincible { get; set; }
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
            _iFrameManager = gameObject.AddComponent<IFrameManager>();
            _iFrameManager.Initialize(this, _sprite);
        }

        public virtual void Damage() {
            if (Invincible) return;
            if (!Data._isKillable) return;
            _iFrameManager.SetIFrames();
            Invincible = true;
            EffectsManager.SpawnEffect(_damageFx.GetFullID, transform.position);
            _anim.Damage();
            if (HP != 0)
                HP -= 1;
            else
                Die();
        }

        protected virtual void Die() {
            IsDead = true;
            _iFrameManager.StopIFrames();
            _sprite.enabled = false;
            EffectsManager.SpawnEffect(_deathFx.GetFullID, transform.position);
            if(_destroyOnDeath) Destroy(gameObject);
        }

        public virtual void Update() {
            if (Data == null) return;
            if (transform.position.y <= _killheight)
                Die();
            if(Data._isKillable)
                _iFrameManager.HandleIFrames();
        }
    }
}