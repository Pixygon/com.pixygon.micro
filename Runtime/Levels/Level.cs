using UnityEngine;

namespace Pixygon.Micro {
    public class Level : MonoBehaviour {
        [SerializeField] private Coin[] _coins;
        [SerializeField] private MicroActorSpawner[] _actors;
        [SerializeField] private Transform _playerSpawn;
        public Vector3 PlayerSpawn => _playerSpawn.position;

        public void RespawnLevel() {
            foreach (var coin in _coins) {
                coin.Respawn();
            }
            foreach (var spawner in _actors) {
                spawner.SpawnActor();
            }
        }
    }
}