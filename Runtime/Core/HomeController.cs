using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pixygon.Micro {
    public class HomeController : MonoBehaviour {
        [SerializeField] private GameObject _homeMenu;
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _settingsMenu;
        [SerializeField] private GameObject _faceplateMenu;
        [SerializeField] private GameObject _cartridgeMenu;
        [SerializeField] private TextMeshProUGUI _versionText;
        [SerializeField] private TextMeshProUGUI _gameTitleText;
        [SerializeField] private GameObject _eventHomeTest;
        [SerializeField] private GameObject _eventSettingsTest;
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _bgmSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private TextMeshProUGUI _masterText;
        [SerializeField] private TextMeshProUGUI _bgmText;
        [SerializeField] private TextMeshProUGUI _sfxText;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private CartridgeSelector _cartridgeSelector;
        [SerializeField] private FaceplateSelector _faceplateSelector;
        
        [SerializeField] private HomeAccountScreen _homeAccountScreen;

        public void Initialize() {
            GetComponent<Canvas>().worldCamera = MicroController._instance.Display._uiCamera;
            GetComponent<Canvas>().sortingLayerName = "Menu";
            _versionText.text = MicroController._instance.Version;
            SetCurrentCartridge();
            _masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 1f)*10);
            _bgmSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("BGMVolume", 1f)*10);
            _sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFXVolume", 1f)*10);
            _masterText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("MasterVolume", 1f)*100f)}%";
            _bgmText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("BGMVolume", 1f)*100f)}%";
            _sfxText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("SFXVolume", 1f)*100f)}%";
        }
        public void Activate(bool activate) {
            _homeMenu.SetActive(activate);
            _mainMenu.SetActive(activate);
            _settingsMenu.SetActive(false);
            _cartridgeMenu.SetActive(false);
            _faceplateMenu.SetActive(false);
            _homeAccountScreen.gameObject.SetActive(false);
            if(activate)
                EventSystem.current.SetSelectedGameObject(_eventHomeTest);
            else
                MicroController._instance.SetCameraToDefault();
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
        
        public void TriggerSettingsMenu(bool open) {
            _settingsMenu.SetActive(open);
            _mainMenu.SetActive(!open);
            EventSystem.current.SetSelectedGameObject(open ? _eventSettingsTest : _eventHomeTest);
        }
        public void TriggerFaceplateSelect(bool open) {
            _mainMenu.SetActive(!open);
            if (open) _faceplateSelector.Open();
        }
        public void TriggerCartridgeSelect(bool open) {
            _mainMenu.SetActive(!open);
            _cartridgeMenu.SetActive(open);
            if (open) _cartridgeSelector.Open();
        }

        public void SetCurrentCartridge() {
            var hasCartridge = MicroController._instance.CurrentlyLoadedCartridge != null;
            _gameTitleText.text = hasCartridge ? MicroController._instance.CurrentlyLoadedCartridge._title : "No cartridge";
        }

        public void TriggerAccountMenu(bool open) {
            _mainMenu.SetActive(!open);
            if(!open)
                EventSystem.current.SetSelectedGameObject(_eventHomeTest);
            else
                _homeAccountScreen.OpenScreen(true);
        }

        public void StartGame() {
            if (MicroController._instance.CurrentlyLoadedCartridge == null) return;
            MicroController._instance.Cartridge.StartGame();
            MicroController._instance.CloseHomeMenu();
            Activate(false);
        }

    }
}