using UnityEngine;

namespace Pixygon.Micro.Parallax {
    public class Parallax : MonoBehaviour {
        [SerializeField] private ParallaxLayer _parallaxLayerPrefab;
        
        public ParallaxLayer[] ParallaxLayers { get; private set; }
        private bool _loaded;
        
        public void Initialize(Transform player, Camera camera, ParallaxLayerData[] layerData) {
            _loaded = false;
            if (ParallaxLayers != null)
                ResetParallax();
            ParallaxLayers = new ParallaxLayer[layerData.Length];
            for (var i = 0; i < layerData.Length; i++) {
                ParallaxLayers[i] = Instantiate(_parallaxLayerPrefab, transform);
                ParallaxLayers[i].Initialize(player, camera, layerData[i]);
            }
            _loaded = true;
        }

        private void ResetParallax() {
            foreach (var g in ParallaxLayers) {
                Destroy(g.gameObject);
            }
            ParallaxLayers = null;
        }
        
        private void Update() {
            if (!_loaded) return;
            foreach (var layer in ParallaxLayers) {
                layer.UpdateParallax();
            }
        }
    }
}