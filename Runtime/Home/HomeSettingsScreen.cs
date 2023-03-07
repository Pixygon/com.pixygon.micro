using System.Collections;
using System.Collections.Generic;
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
        
        [SerializeField] private GameObject _eventSettings;
        public void OpenScreen(bool open) {
            gameObject.SetActive(open);
            if (!open) {
                _homeScreen.TriggerAccountMenu(false);
                return;
            }
            EventSystem.current.SetSelectedGameObject(_eventSettings);
            SetSettingsScreen();
        }
        
        public void SetSettingsScreen() {
            _versionText.text = MicroController._instance.Version;
            _masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 1f)*10);
            _bgmSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("BGMVolume", 1f)*10);
            _sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFXVolume", 1f)*10);
            _masterText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("MasterVolume", 1f)*100f)}%";
            _bgmText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("BGMVolume", 1f)*100f)}%";
            _sfxText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("SFXVolume", 1f)*100f)}%";
            EventSystem.current.SetSelectedGameObject(_eventSettings);
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
    }
}
