using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pixygon.Micro
{
    public class HomeSettingsScreen : MonoBehaviour
    {
        [SerializeField] private HomeController _homeScreen;
        
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _bgmSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private TextMeshProUGUI _masterText;
        [SerializeField] private TextMeshProUGUI _bgmText;
        [SerializeField] private TextMeshProUGUI _sfxText;
        [SerializeField] private TextMeshProUGUI _versionText;

        [SerializeField] private GameObject _audioScreen;
        [SerializeField] private GameObject _visualsScreen;
        [SerializeField] private GameObject _bindingsScreen;
        
        [SerializeField] private GameObject _eventAudioSettings;
        [SerializeField] private GameObject _eventVisualsScreen;
        [SerializeField] private GameObject _eventBindingsScreen;
        [SerializeField] private Slider _zoomSlider;
        [SerializeField] private Slider _pitchSlider;
        [SerializeField] private Slider _yawSlider;
        [SerializeField] private TextMeshProUGUI _zoomText;
        [SerializeField] private TextMeshProUGUI _pitchText;
        [SerializeField] private TextMeshProUGUI _yawText;
        
        [SerializeField] private GameObject _eventSettings;
        public void OpenScreen(bool open) {
            gameObject.SetActive(open);
            if (!open) {
                _homeScreen.TriggerAccountMenu(false);
                return;
            }
            _homeScreen.SelectSfx.Play();
            EventSystem.current.SetSelectedGameObject(_eventSettings);
            SetSettingsScreen();
        }

        private void OnEnable() {
            MicroController._instance.Input._run += DoClose;
        }

        private void OnDisable() {
            MicroController._instance.Input._run -= DoClose;
        }
        public void OpenAudio(bool open) {
            _audioScreen.SetActive(open);
            EventSystem.current.SetSelectedGameObject(open ? _eventAudioSettings : _eventSettings);
            if (!open) return;
            _masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 1f)*10);
            _bgmSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("BGMVolume", 1f)*10);
            _sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFXVolume", 1f)*10);
            _masterText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("MasterVolume", 1f)*100f)}%";
            _bgmText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("BGMVolume", 1f)*100f)}%";
            _sfxText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("SFXVolume", 1f)*100f)}%";
        }
        public void OpenVisuals(bool open) {
            _visualsScreen.SetActive(open);
            EventSystem.current.SetSelectedGameObject(open ? _eventVisualsScreen : _eventSettings);
            if (!open) return;
            _zoomSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("Visual Zoom", .5f));
            _pitchSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("Visual Pitch", 0f));
            _yawSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("Visual Yaw", 0f));
            _zoomText.text = $"{Mathf.Round(PlayerPrefs.GetFloat("Visual Zoom", .5f)*100f)}%";
            _pitchText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("Visual Pitch", 0f)*10f)}";
            _yawText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("Visual Yaw", 0f)*10f)}";
        }
        public void OpenBindings(bool open) {
            _bindingsScreen.SetActive(open);
            EventSystem.current.SetSelectedGameObject(open ? _eventBindingsScreen : _eventSettings);
        }
        public void SetSettingsScreen() {
            _versionText.text = MicroController._instance.Version;
            EventSystem.current.SetSelectedGameObject(_eventSettings);
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
        public void SetMasterAudioLevel(float f) {
            PlayerPrefs.SetFloat("MasterVolume", Mathf.Clamp(f * .1f, 0.0001f, 1f));
            PlayerPrefs.Save();
            _masterText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("MasterVolume", 1f)*100f)}%";
            MicroController._instance.UpdateAudioSettings();
        }
        public void SetBgmAudioLevel(float f) {
            PlayerPrefs.SetFloat("BGMVolume", Mathf.Clamp(f * .1f, 0.0001f, 1f));
            PlayerPrefs.Save();
            _bgmText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("BGMVolume", 1f)*100f)}%";
            MicroController._instance.UpdateAudioSettings();
        }
        public void SetSfxAudioLevel(float f) {
            PlayerPrefs.SetFloat("SFXVolume", Mathf.Clamp(f * .1f, 0.0001f, 1f));
            PlayerPrefs.Save();
            _sfxText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("SFXVolume", 1f)*100f)}%";
            MicroController._instance.UpdateAudioSettings();
        }
        private void DoClose(bool started) {
            if (!started) return;
            _homeScreen.BackSfx.Play();
        }
    }
}
