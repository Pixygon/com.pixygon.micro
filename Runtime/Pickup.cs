using UnityEngine;
using UnityEngine.Events;

namespace Pixygon.Micro {
    public class Pickup : MonoBehaviour {
        //[SerializeField] private ParticleSystem _effect;
        [SerializeField] private UnityEvent _onPickup;
        [SerializeField] private UnityEvent _onRespawn;
        [SerializeField] private bool _destroyOnPickup;
        [SerializeField] private float _destroyTime;
        
        [field: SerializeField] public int PickupId { get; private set; }
        
        public bool Taken { get; private set; }
        public void TakePickup() {
            //GetComponent<Collider2D>().enabled = false;
            //GetComponent<SpriteRenderer>().enabled = false;
            Taken = true;
            _onPickup.Invoke();
            if (_destroyOnPickup)
                Destroy(gameObject, _destroyTime);
            //_effect.Play();
        }

        public void Respawn() {
            //GetComponent<Collider2D>().enabled = true;
            //GetComponent<SpriteRenderer>().enabled = true;
            Taken = false;
            _onRespawn.Invoke();
        }
    }
}