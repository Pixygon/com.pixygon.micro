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
            Log.DebugMessage(DebugGroup.Actor, "Spawning actor", this);
            var a = await AddressableLoader.LoadGameObject(_actorData._actorRef, transform);
            a.transform.localPosition = Vector3.zero;
            a.GetComponent<MicroActor>().Initialize(_loader, _actorData);
            _spawnedActor.Add(a);
        }

        private void DoSpawn() {
            if(_onlyOneActor) DespawnActor();
            _timer = _spawnTimer;
            SpawnActor();
        }
        private void DespawnActor() {
            foreach (var a in _spawnedActor) {
                Destroy(a);
            }
        }

        private void Update() {
            if (!_repeat || !_initialized) return;
            if (_timer < 0f) DoSpawn();
            else _timer -= Time.deltaTime;
        }
    }
}