using System.Collections.Generic;
using System.Linq;
using Pixygon.Actors;
using Pixygon.DebugTool;
using Pixygon.Addressable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pixygon.Micro {
    public class MicroActorSpawner : MonoBehaviour {
        [SerializeField] private MicroActorData _actorData;
        [SerializeField] private bool _onlyOneActor = true;
        [SerializeField] private bool _repeat;
        [SerializeField] private int _spawnTimerMin = 150;
        [SerializeField] private int _spawnTimerMax = 300;
        [SerializeField] private int _spawnTimerStart;
        [SerializeField] private ActorPatrolPattern _actorPatrolPattern;
        
        private float _timer;
        private LevelLoader _loader;
        private bool _initialized;
        public List<GameObject> SpawnedActor { get; private set; }
        public bool Pause { get; set; }
        public void Initialize(LevelLoader loader) {
            _loader = loader;
            DeSpawnActor();
            SpawnedActor = new List<GameObject>();
            _initialized = true;
            _timer = _spawnTimerStart;
            if(!_repeat)
                DoSpawn();
        }
        private async void SpawnActor() {
            Log.DebugMessage(DebugGroup.Actor, "Spawning actor", this);
            var a = await AddressableLoader.LoadGameObject(_actorData._actorRef, transform);
            a.transform.localPosition = Vector3.zero;
            if(_actorPatrolPattern != null)
                a.GetComponent<Actors.Actor>().SetPatrolData(_actorPatrolPattern.PatrolData);
            a.GetComponent<MicroActor>().Initialize(_loader, _actorData);
            SpawnedActor.Add(a);
        }
        private void DoSpawn() {
            if(_onlyOneActor) DeSpawnActor();
            PruneActorList();
            _timer = Random.Range(_spawnTimerMin, _spawnTimerMax);
            SpawnActor();
        }
        public void DeSpawnActor() {
            if(SpawnedActor == null) return;
            foreach (var a in SpawnedActor) {
                Destroy(a);
            }
            PruneActorList();
        }
        private void PruneActorList() {
            SpawnedActor = SpawnedActor.Where(a => a != null).ToList();
        }
        private void Update() {
            if (!_repeat || !_initialized || Pause) return;
            if (_timer < 0f) DoSpawn();
            else _timer -= Time.deltaTime;
        }
    }
}