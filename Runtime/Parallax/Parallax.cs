using UnityEngine;

namespace Pixygon.Micro {
    public class Parallax : MonoBehaviour {
        [SerializeField] private ParallaxLayer[] _parallaxLayers;
        
        public void Initialize(Transform player, ParallaxLayerData[] layerData) {
            for (var i = 0; i < _parallaxLayers.Length; i++) {
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