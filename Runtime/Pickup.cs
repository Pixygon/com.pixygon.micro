using Pixygon.Effects;
using UnityEngine;
using UnityEngine.Events;

namespace Pixygon.Micro {
    public class Pickup : MonoBehaviour {
        [SerializeField] private EffectData _effect;
        [SerializeField] private UnityEvent _onPickup;
        [SerializeField] private UnityEvent _onRespawn;
        [SerializeField] private bool _destroyOnPickup;
        [SerializeField] private float _destroyTime;
        [SerializeField] private bool _timeout;
        [SerializeField] private float _timeoutTime = 8f;
        
        [field: SerializeField] public int PickupId { get; private set; }

        private float _timer;
        public bool Taken { get; private set; }
        public void TakePickup() {
            Taken = true;
            _onPickup.Invoke();
            EffectsManager.SpawnEffect(_effect.GetFullID, transform.position);
            if (_destroyOnPickup)
                Destroy(gameObject, _destroyTime);
        }

        public void Respawn() {
            Taken = false;
            _onRespawn.Invoke();
        }

        private void Awake() {
            if (_timeout)
                _timer = _timeoutTime;
        }

        private void Update() {
            if (!_timeout) return;
            _timer -= Time.deltaTime;
            if (_timer < 0f) Destroy(gameObject);
        }
    }
}