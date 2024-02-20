using UnityEngine;

namespace Pixygon.Micro {
    public class ItemSpawner : MonoBehaviour {
        [SerializeField] protected GameObject _itemPrefab;
        [SerializeField] private Vector2 _direction = Vector2.up;
        [SerializeField] private Vector2 _offset = Vector2.up;
        [SerializeField] private float _force = 10f;
        [SerializeField] private bool _followParentRotation;
        [SerializeField] private bool _destroyOnLevelReload;

        public virtual void SpawnItem() {
            var rotation = _followParentRotation ? transform.rotation : Quaternion.identity;
            var g = Instantiate(_itemPrefab, transform.position + (Vector3)_offset, rotation);
            g.GetComponent<Rigidbody2D>().AddForce(_direction * _force, ForceMode2D.Impulse);
            if (_destroyOnLevelReload)
                MicroController._instance.Cartridge.LevelLoader.CurrentLevel._removeOnRestartAction +=
                    () => { Destroy(g); };
        }
        public virtual void SpawnItem(GameObject item) {
            var rotation = _followParentRotation ? transform.rotation : Quaternion.identity;
            var g = Instantiate(item, transform.position + (Vector3)_offset, rotation);
            g.GetComponent<Rigidbody2D>().AddForce(_direction * _force, ForceMode2D.Impulse);
        }
    }
}
