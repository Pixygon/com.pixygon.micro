using UnityEngine;

namespace Pixygon.Micro {
    public class ItemSpawner : MonoBehaviour {
        [SerializeField] protected GameObject _itemPrefab;
        [SerializeField] private Vector2 _direction = Vector2.up;
        [SerializeField] private Vector2 _offset = Vector2.up;
        [SerializeField] private float _force = 10f;

        public virtual void SpawnItem() {
            var g = Instantiate(_itemPrefab, transform.position + (Vector3)_offset, Quaternion.identity);
            g.GetComponent<Rigidbody2D>().AddForce(_direction * _force, ForceMode2D.Impulse);
        }
        public virtual void SpawnItem(GameObject item) {
            var g = Instantiate(item, transform.position + (Vector3)_offset, Quaternion.identity);
            g.GetComponent<Rigidbody2D>().AddForce(_direction * _force, ForceMode2D.Impulse);
        }
    }
}
