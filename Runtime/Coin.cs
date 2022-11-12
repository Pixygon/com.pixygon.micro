using UnityEngine;

namespace Pixygon.Micro {
    public class Pickup : MonoBehaviour {
        [SerializeField] private ParticleSystem _effect;
        
        [field: SerializeField] public int PickupId { get; private set; }
        public bool Taken { get; private set; }
        public void TakePickup() {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Taken = true;
            _effect.Play();
        }

        public void Respawn() {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            Taken = false;
        }
    }
}