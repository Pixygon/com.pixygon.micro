using TMPro;
using UnityEngine;

namespace Pixygon.Micro {
    public class LevelLoadScreen : MonoBehaviour {
        [SerializeField] private TextMeshPro _loadPercentageText;

        public void Activate(bool active) {
            _loadPercentageText.text = "0%";
        }
            
        public void SetLoadPercentage(float f) {
            _loadPercentageText.text = Mathf.RoundToInt(f*100f) + "%";
        }
    }
}