using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pixygon.Micro {
    public class Level : MonoBehaviour {
        [FormerlySerializedAs("_coins")]
        [SerializeField] private Pickup[] _pickups;
        [SerializeField] private MicroActorSpawner[] _actors;
        [SerializeField] private LevelObject[] _levelObjects;
        [SerializeField] private Transform[] _playerSpawns;
        [SerializeField] private bool _useKillHeight;
        [SerializeField] private int _killHeight;

        [SerializeField] private bool _useMissions;
        
        [ContextMenuItem("Get Pickups", "GatherPickups")]
        [ContextMenuItem("Get ActorSpawners", "GatherActors")]
        [ContextMenuItem("Get LevelObjects", "GatherLevelObjects")]
        [SerializeField] private int _currentMission;
        [SerializeField] private LevelMission[] _levelMissions;
        public Transform[] PlayerSpawns => _useMissions ? _levelMissions[CurrentMission]._playerSpawns : _playerSpawns;
        public int KillHeight => _useKillHeight ? _killHeight : -99999;
        public int CurrentMission => _currentMission;
        
        public Action _removeOnRestartAction;

        public void Unload() {
            _removeOnRestartAction?.Invoke();
            _removeOnRestartAction = null;
        }
        public void RespawnLevel(LevelLoader loader, int mission) {
            _currentMission = mission;
            if(_levelMissions[CurrentMission]._missionObject != null)
                _levelMissions[CurrentMission]._missionObject.SetActive(true);
            Unload();
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