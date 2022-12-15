using UnityEngine;


namespace Pixygon.Micro {
    public class TimedItemSpawner : ItemSpawner {
        [SerializeField] private float _timerStart = 10f;

        private float _timer;

        private void Update() {
            if (_timer >= 0f)
                _timer -= Time.deltaTime;
            else {
                SpawnItem();
                _timer = _timerStart;
            }
        }
    }
}