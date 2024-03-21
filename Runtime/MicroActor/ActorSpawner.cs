using System;
using System.Collections.Generic;
using System.Linq;
using Pixygon.Actors;
using Pixygon.DebugTool;
using Pixygon.Addressable;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Pixygon.Micro {
    public class ActorSpawner : MonoBehaviour {
        [SerializeField] private ActorData _actorData;
        [SerializeField] private bool _onlyOneActor = true;
        [SerializeField] private bool _repeat;
        [SerializeField] private int _spawnTimerMin = 150;
        [SerializeField] private int _spawnTimerMax = 300;
        [SerializeField] private int _spawnTimerStart;
        [SerializeField] private PatrolData _patrolData;
        [SerializeField] private UnityEvent _actorOnKill;
        
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
            a.GetComponent<Actors.Actor>().SetPatrolData(_patrolData);
            a.GetComponent<MicroActor>().Initialize(_loader, _actorData, _actorOnKill);
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
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            if (_patrolData._usePatrol) {
                for (var i = 0; i < _patrolData._patrolPointDatas.Length; i++) {
                    var patrolPointData = _patrolData._patrolPointDatas[i]._pos;
                    if (_patrolData._patrolPointDatas[i]._useLocalPos)
                        patrolPointData += transform.position;
                    Gizmos.color = Color.Lerp(Color.green, Color.red, (float)i / _patrolData._patrolPointDatas.Length);
                    Handles.Label(patrolPointData, $"({i})");
                    if (i == 0) {
                        ForGizmo(patrolPointData,
                            Vector3.Normalize(patrolPointData - _patrolData._patrolPointDatas[_patrolData._patrolPointDatas.Length-1]._pos));
                    } else {
                        ForGizmo(patrolPointData, Vector3.Normalize(patrolPointData - _patrolData._patrolPointDatas[i-1]._pos));
                    }
                    if (i == _patrolData._patrolPointDatas.Length-1) {
                        Gizmos.DrawLine(patrolPointData, _patrolData._patrolPointDatas[0]._pos);
                    } else {
                        Gizmos.DrawLine(patrolPointData, _patrolData._patrolPointDatas[i+1]._useLocalPos ? _patrolData._patrolPointDatas[i+1]._pos + transform.position : _patrolData._patrolPointDatas[i+1]._pos);
                    }
                }
            }
        }
        private static void ForGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f) {
            if (direction == Vector3.zero) return;
            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(180+arrowHeadAngle, 0, 0) * Vector3.forward;
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(180-arrowHeadAngle, 0,0) * Vector3.forward;
            Gizmos.DrawRay(pos, right * arrowHeadLength);
            Gizmos.DrawRay(pos, left * arrowHeadLength);
        }
        #endif
    }
}