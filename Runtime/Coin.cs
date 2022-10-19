using UnityEngine;

namespace Pixygon.Micro {
    public class Coin : MonoBehaviour {
        public void TakeCoin() {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }

        public void Respawn() {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}