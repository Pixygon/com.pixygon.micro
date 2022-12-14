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
        [SerializeField] private Transform _playerSpawn;
        [SerializeField] private int _killHeight;
        public Vector3 PlayerSpawn => _playerSpawn.position;
        public int KillHeight => _killHeight;

        public void RespawnLevel(LevelLoader loader) {
            foreach (var coin in _pickups) {
                coin.Respawn();
            }
            foreach (var spawner in _actors) {
                spawner.Initialize(loader);
            }
            foreach (var levelObject in _levelObjects) {
                levelObject.Reset();
            }
        }

        private void GatherPickups() {
            _pickups = GetComponentsInChildren<Pickup>();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        private void GatherActors() {
            _actors = GetComponentsInChildren<MicroActorSpawner>();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        private void GatherLevelObjects() {
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