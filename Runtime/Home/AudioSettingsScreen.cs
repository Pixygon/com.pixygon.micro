using System.Collections;
using System.Collections.Generic;
using Pixygon.Addressable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pixygon.Micro {
    public class AudioSettingsScreen : MonoBehaviour
    {
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _bgmSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private TextMeshProUGUI _masterText;
        [SerializeField] private TextMeshProUGUI _bgmText;
        [SerializeField] private TextMeshProUGUI _sfxText;
        
        public void OpenScreen(bool open) {
            gameObject.SetActive(open);
            if (!open) return;
            _masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 1f)*10);
            _bgmSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("BGMVolume", 1f)*10);
            _sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFXVolume", 1f)*10);
            _masterText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("MasterVolume", 1f)*100f)}%";
            _bgmText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("BGMVolume", 1f)*100f)}%";
            _sfxText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("SFXVolume", 1f)*100f)}%";
        }
        public void SetMasterAudioLevel(float f) {
            PlayerPrefs.SetFloat("MasterVolume", Mathf.Clamp(f * .1f, 0.0001f, 1f));
            PlayerPrefs.Save();
            _masterText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("MasterVolume", 1f)*100f)}%";
            AudioMaster.Instance.SetMixer();
        }
        public void SetBgmAudioLevel(float f) {
            PlayerPrefs.SetFloat("BGMVolume", Mathf.Clamp(f * .1f, 0.0001f, 1f));
            PlayerPrefs.Save();
            _bgmText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("BGMVolume", 1f)*100f)}%";
            AudioMaster.Instance.SetMixer();
        }
        public void SetSfxAudioLevel(float f) {
            PlayerPrefs.SetFloat("SFXVolume", Mathf.Clamp(f * .1f, 0.0001f, 1f));
            PlayerPrefs.Save();
            _sfxText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("SFXVolume", 1f)*100f)}%";
            AudioMaster.Instance.SetMixer();
        }
    }
}
