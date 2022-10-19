using UnityEngine;

namespace Pixygon.Micro {
    public class Parallax : MonoBehaviour {
        [SerializeField] private ParallaxLayer _parallaxLayerPrefab;
        
        private ParallaxLayer[] _parallaxLayers;
        
        public void Initialize(Transform player, ParallaxLayerData[] layerData) {
            _parallaxLayers = new ParallaxLayer[layerData.Length];
            for (var i = 0; i < layerData.Length; i++) {
                _parallaxLayers[i] = Instantiate(_parallaxLayerPrefab, transform);
                _parallaxLayers[i].Initialize(player, layerData[i]);
            }
        }
        
        private void Update() {
            foreach (var layer in _parallaxLayers) {
                layer.UpdateParallax();
            }
        }
    }
}