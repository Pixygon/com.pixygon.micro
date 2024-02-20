using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pixygon.Micro {
    public class Level : MonoBehaviour {
        [FormerlySerializedAs("_coins")]
        [ContextMenuItem("Get Pickups", "GatherPickups")]
        [SerializeField] private Pickup[] _pickups;
        [ContextMenuItem("Get ActorSpawners", "GatherActors")]
        [SerializeField] private MicroActorSpawner[] _actors;
        [ContextMenuItem("Get LevelObjects", "GatherLevelObjects")]
        [SerializeField] private LevelObject[] _levelObjects;
        [SerializeField] private Transform[] _playerSpawns;
        [SerializeField] private int _killHeight;

        [SerializeField] private bool _useMissions;
        [SerializeField] private LevelMission[] _levelMissions;
        public Transform[] PlayerSpawns => _useMissions ? _levelMissions[CurrentMission]._playerSpawns : _playerSpawns;
        public int KillHeight => _killHeight;
        public int CurrentMission => _currentMission;
        
        public Action _removeOnRestartAction;

        private int _currentMission;

        public void RespawnLevel(LevelLoader loader) {
            _removeOnRestartAction?.Invoke();
            _removeOnRestartAction = null;
            if (!_useMissions) {
                foreach (var coin in _pickups) {
                    coin.Respawn();
                }
                foreach (var spawner in _actors) {
                    spawner.Initialize(loader);
                }
                foreach (var levelObject in _levelObjects) {
                    levelObject.Reset();
                }
            } else {
                foreach (var coin in _levelMissions[CurrentMission]._pickups) {
                    coin.Respawn();
                }
                foreach (var spawner in _levelMissions[CurrentMission]._actors) {
                    spawner.Initialize(loader);
                }
                foreach (var levelObject in _levelMissions[CurrentMission]._levelObjects) {
                    levelObject.Reset();
                }
            }
        }

        private void GatherPickups() {
            if(_useMissions)
                _levelMissions[CurrentMission]._pickups = GetComponentsInChildren<Pickup>();
            else
                _pickups = GetComponentsInChildren<Pickup>();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        private void GatherActors() {
            if(_useMissions)
                _levelMissions[CurrentMission]._actors = GetComponentsInChildren<MicroActorSpawner>();
            else
                _actors = GetComponentsInChildren<MicroActorSpawner>();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        private void GatherLevelObjects() {
            if(_useMissions)
                _levelMissions[CurrentMission]._levelObjects = GetComponentsInChildren<LevelObject>();
            else
                _levelObjects = GetComponentsInChildren<LevelObject>();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(-1000, _killHeight, 0), new Vector3(1000, _killHeight, 0));
        }
    }
}