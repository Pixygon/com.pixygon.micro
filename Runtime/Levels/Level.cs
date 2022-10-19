using UnityEngine;

namespace Pixygon.Micro {
    public class Level : MonoBehaviour {
        [SerializeField] private Coin[] _coins;
        [SerializeField] private MicroActorSpawner[] _actors;
        [SerializeField] private Transform _playerSpawn;
        [SerializeField] private ParallaxLayerData[] _parallaxLayers;
        //[SerializeField] private Parallax _parallax;
        public Vector3 PlayerSpawn => _playerSpawn.position;
        public ParallaxLayerData[] ParallaxLayerDatas => _parallaxLayers;

        /*
        public void InitializeLevel() {
            _parallax.Initialize(_parallaxLayers);
        }
        */
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