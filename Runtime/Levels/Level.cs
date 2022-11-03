using UnityEngine;

namespace Pixygon.Micro {
    public class Level : MonoBehaviour {
        [ContextMenuItem("Get Pickups", "GatherPickups")]
        [SerializeField] private Coin[] _coins;
        [ContextMenuItem("Get ActorSpawners", "GatherActors")]
        [SerializeField] private MicroActorSpawner[] _actors;
        [SerializeField] private Transform _playerSpawn;
        public Vector3 PlayerSpawn => _playerSpawn.position;

        public void RespawnLevel(LevelLoader loader) {
            foreach (var coin in _coins) {
                coin.Respawn();
            }
            foreach (var spawner in _actors) {
                spawner.SpawnActor(loader);
            }
        }

        private void GatherPickups() {
            _coins = GetComponentsInChildren<Coin>();
        }

        private void GatherActors() {
            _actors = GetComponentsInChildren<MicroActorSpawner>();
        }
    }
}