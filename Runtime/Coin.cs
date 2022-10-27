using UnityEngine;

namespace Pixygon.Micro {
    public class Coin : MonoBehaviour {
        [SerializeField] private ParticleSystem _effect;
        public void TakeCoin() {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            _effect.Play();
        }

        public void Respawn() {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            _effect.Play();
        }
    }
}