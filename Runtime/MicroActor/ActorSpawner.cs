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
            var lastPos = transform.position;
            PatrolPointData prevPatrolPoint = null;
            PatrolPointData nextPatrolPoint = null;
            if (!_patrolData._usePatrol) return;
            for (var i = 0; i < _patrolData._patrolPointDatas.Length; i++) {
                var currentPoint = _patrolData._patrolPointDatas[i];
                if (currentPoint._onlyLook) {
                    Gizmos.color = Color.blue;
                    Handles.Label(currentPoint._pos, "<o>");
                    ForGizmo(currentPoint._pos, Vector3.Normalize(currentPoint._pos- lastPos));
                    Gizmos.DrawLine(lastPos, currentPoint._pos);
                }
                else {
                    var currentPos = currentPoint._pos;
                    if (!currentPoint._useWorldPos)
                        currentPos += transform.position;
                    lastPos = currentPos;
                
                    var nextPoint = i == _patrolData._patrolPointDatas.Length-1 ? _patrolData._patrolPointDatas[0] : _patrolData._patrolPointDatas[i+1];
                    var nextPos = nextPoint._pos;
                    if (!nextPoint._useWorldPos)
                        nextPos += transform.position;
                
                    var prevPoint = i == 0 ? _patrolData._patrolPointDatas[^1] : _patrolData._patrolPointDatas[i - 1];
                    var prevPos = prevPoint._pos;
                    if (!prevPoint._useWorldPos)
                        prevPos += transform.position;
                
                    Gizmos.color = Color.Lerp(Color.green, Color.red, (float)i / _patrolData._patrolPointDatas.Length);
                    Handles.Label(currentPos, $"({i})");
                    ForGizmo(currentPos, Vector3.Normalize(currentPos - prevPos));
                    Gizmos.DrawLine(currentPos, nextPos);
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