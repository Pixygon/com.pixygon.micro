using UnityEngine;

namespace Pixygon.Micro {
    public class Parallax : MonoBehaviour {
        [SerializeField] private ParallaxLayer _parallaxLayerPrefab;
        
        private ParallaxLayer[] _parallaxLayers;
        private bool _loaded;
        
        public void Initialize(Transform player, ParallaxLayerData[] layerData) {
            _loaded = false;
            if (_parallaxLayers != null)
                ResetParallax();
            _parallaxLayers = new ParallaxLayer[layerData.Length];
            for (var i = 0; i < layerData.Length; i++) {
                _parallaxLayers[i] = Instantiate(_parallaxLayerPrefab, transform);
                _parallaxLayers[i].Initialize(player, layerData[i]);
            }
            _loaded = true;
        }

        private void ResetParallax() {
            foreach (var g in _parallaxLayers) {
                Destroy(g.gameObject);
            }
            _parallaxLayers = null;
        }
        
        private void Update() {
            if (!_loaded) return;
            foreach (var layer in _parallaxLayers) {
                layer.UpdateParallax();
            }
        }
    }
}