using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pixygon.Micro {
    public class DisplaySettingsScreen : MonoBehaviour{
        [SerializeField] private Slider _zoomSlider;
        [SerializeField] private Slider _pitchSlider;
        [SerializeField] private Slider _yawSlider;
        [SerializeField] private TextMeshProUGUI _zoomText;
        [SerializeField] private TextMeshProUGUI _pitchText;
        [SerializeField] private TextMeshProUGUI _yawText;
        
        public void OpenScreen(bool open) {
            gameObject.SetActive(open);
            if (!open) return;
            _zoomSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("Visual Zoom", .5f));
            _pitchSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("Visual Pitch", 0f));
            _yawSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("Visual Yaw", 0f));
            _zoomText.text = $"{Mathf.Round(PlayerPrefs.GetFloat("Visual Zoom", .5f)*100f)}%";
            _pitchText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("Visual Pitch", 0f)*10f)}";
            _yawText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("Visual Yaw", 0f)*10f)}";
        }
        public void SetVisualZoom(float f) {
            PlayerPrefs.SetFloat("Visual Zoom", Mathf.Clamp(f, 0f, 1f));
            PlayerPrefs.Save();
            _zoomText.text = $"{Mathf.Round(PlayerPrefs.GetFloat("Visual Zoom", .5f)*100f)}%";
            MicroController._instance.UpdateVisualSettings();
        }
        public void SetVisualPitch(float f) {
            PlayerPrefs.SetFloat("Visual Pitch", Mathf.Clamp(f, -8f, 8f));
            PlayerPrefs.Save();
            _pitchText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("Visual Pitch", 0f)*10f)}";
            MicroController._instance.UpdateVisualSettings();
        }
        public void SetVisualYaw(float f) {
            PlayerPrefs.SetFloat("Visual Yaw", Mathf.Clamp(f, -8f, 8f));
            PlayerPrefs.Save();
            _yawText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("Visual Yaw", 0f)*10f)}";
            MicroController._instance.UpdateVisualSettings();
        }
    }
}