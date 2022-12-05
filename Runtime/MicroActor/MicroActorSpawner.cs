using System;
using System.Collections.Generic;
using Pixygon.DebugTool;
using Pixygon.Addressable;
using UnityEngine;

namespace Pixygon.Micro {
    public class MicroActorSpawner : MonoBehaviour {
        [SerializeField] private MicroActorData _actorData;
        [SerializeField] private bool _onlyOneActor = true;
        
        [SerializeField] private bool _repeat;
        [SerializeField] private int _spawnTimer;
        
        private List<GameObject> _spawnedActor;
        private float _timer;
        private LevelLoader _loader;
        private bool _initialized;
        public void Initialize(LevelLoader loader) {
            _loader = loader;
            _spawnedActor = new List<GameObject>();
            _initialized = true;
        }
        public async void SpawnActor() {
            if(_onlyOneActor) DespawnActor();
            Log.DebugMessage(DebugGroup.Actor, "Spawning actor", this);
            var a = await AddressableLoader.LoadGameObject(_actorData._actorRef, transform);
            a.transform.localPosition = Vector3.zero;
            a.GetComponent<MicroActor>().Initialize(_loader, _actorData);
            _spawnedActor.Add(a);
            _timer = _spawnTimer;
        }

        private void DespawnActor() {
            foreach (var a in _spawnedActor) {
                Destroy(a);
            }
        }

        private void Update() {
            if (!_repeat || !_initialized) return;
            if (_timer < 0f) SpawnActor();
        }
    }
}