using System;
using Pixygon.DebugTool;
using Pixygon.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pixygon.Micro {
    public class Level : MonoBehaviour {
        [FormerlySerializedAs("_coins")]
        [SerializeField] private Pickup[] _pickups;
        [SerializeField] private ActorSpawner[] _actors;
        [SerializeField] private LevelObject[] _levelObjects;
        [SerializeField] private Transform[] _playerSpawns;
        [SerializeField] private bool _useKillHeight;
        [SerializeField] private int _killHeight;
        [SerializeField] private SplashScreen _splashScreen;

        [SerializeField] private bool _useMissions;
        
        [ContextMenuItem("Get Pickups", "GatherPickups")]
        [ContextMenuItem("Get ActorSpawners", "GatherActors")]
        [ContextMenuItem("Get LevelObjects", "GatherLevelObjects")]
        [SerializeField] private LevelMission[] _levelMissions;
        public Transform[] PlayerSpawns => _useMissions ? _levelMissions[CurrentMission]._playerSpawns : _playerSpawns;
        public int KillHeight => _useKillHeight ? _killHeight : -99999;
        public int CurrentMission => MicroController._instance.Cartridge.LevelLoader.SelectedMission;
        public MissionData CurrentMissionData => _levelMissions[CurrentMission]._connectedMission;
        public Action _removeOnRestartAction;
        public SplashScreen SplashScreen => _splashScreen;

        public void Unload() {
            _removeOnRestartAction?.Invoke();
            _removeOnRestartAction = null;
        }
        public void RespawnLevel(LevelLoader loader) {
            foreach (var mission in _levelMissions) {
                mission._missionObject.SetActive(false);
            }
            if(_levelMissions[CurrentMission]._missionObject != null)
                _levelMissions[CurrentMission]._missionObject.SetActive(true);
            Unload();
            if (!_useMissions) {
                foreach (var pickup in _pickups) {
                    if (pickup == null) {
                        Log.DebugMessage(DebugGroup.PixygonMicro, "Missing Pickup in level", this);
                        continue;    
                    }
                    pickup.Respawn();
                }
                foreach (var spawner in _actors) {
                    if (spawner == null) {
                        Log.DebugMessage(DebugGroup.PixygonMicro, "Missing Spawner in level", this);
                        continue;    
                    }
                    spawner.Initialize(loader);
                }
                foreach (var levelObject in _levelObjects) {
                    if (levelObject == null) {
                        Log.DebugMessage(DebugGroup.PixygonMicro, "Missing LevelObject in level", this);
                        continue;    
                    }
                    levelObject.Reset();
                }
            } else {
                foreach (var pickup in _levelMissions[CurrentMission]._pickups) {
                    if (pickup == null) {
                        Log.DebugMessage(DebugGroup.PixygonMicro, "Missing Pickup in level", this);
                        continue;    
                    }
                    pickup.Respawn();
                }
                foreach (var spawner in _levelMissions[CurrentMission]._actors) {
                    if (spawner == null) {
                        Log.DebugMessage(DebugGroup.PixygonMicro, "Missing Spawner in level", this);
                        continue;    
                    }
                    spawner.Initialize(loader);
                }
                foreach (var levelObject in _levelMissions[CurrentMission]._levelObjects) {
                    if (levelObject == null) {
                        Log.DebugMessage(DebugGroup.PixygonMicro, "Missing LevelObject in level", this);
                        continue;    
                    }
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
                _levelMissions[CurrentMission]._actors = GetComponentsInChildren<ActorSpawner>();
            else
                _actors = GetComponentsInChildren<ActorSpawner>();
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