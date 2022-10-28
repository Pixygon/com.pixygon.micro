using UnityEngine;

namespace Pixygon.Micro {
    public class Coin : MonoBehaviour {
        [SerializeField] private ParticleSystem _effect;
        
        public bool Taken { get; private set; }
        public void TakeCoin() {
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