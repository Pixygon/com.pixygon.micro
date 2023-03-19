using System;
using UnityEngine;
using UnityEngine.Events;

namespace Pixygon.Micro {
    public class RegenerativeItemSpawner : ItemSpawner {
        [SerializeField] private float _timerStart = 10f;
        [SerializeField] private float _timerLength = 10f;
        [SerializeField] private UnityEvent _itemReadyEvent;
        [SerializeField] private UnityEvent _itemNotReadyEvent;
        private bool _itemReady;
        private float _timer;
        
        private void Awake() {
            _timer = _timerStart;
        }
        private void Update() {
            if (_itemReady) return;
            if (_timer >= 0f)
                _timer -= Time.deltaTime;
            else {
                _itemReadyEvent.Invoke();
                _itemReady = true;
            }
        }

        public void GetItem() {
            if (!_itemReady) return;
            _itemReady = false;
            SpawnItem();
            _timer = _timerLength;
            _itemNotReadyEvent.Invoke();
        }
    }
}
