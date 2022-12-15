using UnityEngine;

namespace Pixygon.Micro {
    public class RandomizedItemSpawner : ItemSpawner {
        [SerializeField] private GameObject[] _spawnableItems;
        [SerializeField] private float _spawnChance;

        public override void SpawnItem() {
            if (Random.value < _spawnChance) return;
            _itemPrefab = _spawnableItems[Random.Range(0, _spawnableItems.Length)];
            base.SpawnItem();
        }
    }
}
