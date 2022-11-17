using System.Collections.Generic;
using Pixygon.PagedContent;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pixygon.Micro {
    public class HomeController : MonoBehaviour {
        [SerializeField] private GameObject _homeMenu;
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _settingsMenu;
        [SerializeField] private GameObject _faceplateMenu;
        [SerializeField] private GameObject _cartridgeMenu;
        [SerializeField] private GameObject _trophyMenu;
        [SerializeField] private TextMeshProUGUI _versionText;
        [SerializeField] private TextMeshProUGUI _gameTitleText;
        [SerializeField] private GameObject _eventHomeTest;
        [SerializeField] private GameObject _eventSettingsTest;
        [SerializeField] private GameObject _eventCartridgeTest;
        [SerializeField] private GameObject _eventFaceplateTest;
        [SerializeField] private GameObject _eventTrophiesTest;
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _bgmSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private TextMeshProUGUI _masterText;
        [SerializeField] private TextMeshProUGUI _bgmText;
        [SerializeField] private TextMeshProUGUI _sfxText;
        [SerializeField] private Button _startGameButton;

        [SerializeField] private AudioMixer _mixer;
        public void Initialize() {
            GetComponent<Canvas>().worldCamera = MicroController._instance.Display._uiCamera;
            GetComponent<Canvas>().sortingLayerName = "Menu";
            _versionText.text = MicroController._instance.Version;
            SetCurrentCartridge();
            _masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 1f)*10);
            _bgmSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("BGMVolume", 1f)*10);
            _sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFXVolume", 1f)*10);
            UpdateAudioSettings();
        }
        public void Activate(bool activate) {
            _homeMenu.SetActive(activate);
            _mainMenu.SetActive(activate);
            _settingsMenu.SetActive(false);
            _cartridgeMenu.SetActive(false);
            _faceplateMenu.SetActive(false);
            _trophyMenu.SetActive(false);
            if(activate)
                EventSystem.current.SetSelectedGameObject(_eventHomeTest);
        }

        public void SetMasterAudioLevel(float f) {
            PlayerPrefs.SetFloat("MasterVolume", Mathf.Clamp(f * .1f, 0.0001f, 1f));
            PlayerPrefs.Save();
            UpdateAudioSettings();
        }
        public void SetBgmAudioLevel(float f) {
            PlayerPrefs.SetFloat("BGMVolume", Mathf.Clamp(f * .1f, 0.0001f, 1f));
            PlayerPrefs.Save();
            UpdateAudioSettings();
        }
        public void SetSfxAudioLevel(float f) {
            PlayerPrefs.SetFloat("SFXVolume", Mathf.Clamp(f * .1f, 0.0001f, 1f));
            PlayerPrefs.Save();
            UpdateAudioSettings();
        }
        public void UpdateAudioSettings() {
            _mixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume", 1f)) * 20f);
            _mixer.SetFloat("BGMVolume", Mathf.Log10(PlayerPrefs.GetFloat("BGMVolume", 1f)) * 20f);
            _mixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 1f)) * 20f);
            _masterText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("MasterVolume", 1f)*100f)}%";
            _bgmText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("BGMVolume", 1f)*100f)}%";
            _sfxText.text = $"{Mathf.RoundToInt(PlayerPrefs.GetFloat("SFXVolume", 1f)*100f)}%";
        }
        
        public void TriggerSettingsMenu(bool open) {
            _settingsMenu.SetActive(open);
            _mainMenu.SetActive(!open);
            EventSystem.current.SetSelectedGameObject(open ? _eventSettingsTest : _eventHomeTest);
        }

        //private List<GameObject> _faceplateList;
        //[SerializeField] private GameObject _faceplatePrefab;
        //[SerializeField] private Transform _faceplateParent;
        
        public void TriggerFaceplateSelect(bool open) {
            _faceplateMenu.SetActive(open);
            _mainMenu.SetActive(!open);
            if(open)
                PopulateFaceplateList();
            EventSystem.current.SetSelectedGameObject(open ? _eventFaceplateTest : _eventHomeTest);
        }

        [SerializeField] private PagedContentManager _faceplateLists;
        private void PopulateFaceplateList() {
            _faceplateLists.PopulateCollections(MicroController._instance.Faceplates, 0, SetFaceplate, 0, true);
            /*
            if (_faceplateList != null) {
                foreach (var g in _faceplateList)
                    Destroy(g);
            }
            _faceplateList = new List<GameObject>();
            foreach (var faceplate in MicroController._instance.Faceplates) {
                var g = Instantiate(_faceplatePrefab, _faceplateParent);
                g.GetComponent<FaceplateListing>().SetTitle(faceplate._title);
                _faceplateList.Add(g);
            }
            */
            //_eventFaceplateTest = _faceplateLists.[0];
        }
        public void SetFaceplate(object o, int i) {
            Debug.Log("Set faceplate!! " + i);
            if(i > MicroController._instance.Faceplates.Length)
                i = 0;
            PlayerPrefs.SetInt("Faceplate", i);
            PlayerPrefs.Save();
            MicroController._instance.Console.UpdateFaceplate();
        }


        public void TriggerCartridgeSelect(bool open) {
            _cartridgeMenu.SetActive(open);
            _mainMenu.SetActive(!open);
            EventSystem.current.SetSelectedGameObject(open ? _eventCartridgeTest : _eventHomeTest);
        }
        public void SetCartridge(int i) {
            if(i > MicroController._instance.Cartridges.Length)
                i = 0;
            PlayerPrefs.SetInt("Cartridge", i);
            PlayerPrefs.Save();
            SetCurrentCartridge();
            TriggerCartridgeSelect(false);
        }

        private void SetCurrentCartridge() {
            var hasCartridge = MicroController._instance.CurrentlyLoadedCartridge != null;
            _gameTitleText.text = hasCartridge ? MicroController._instance.CurrentlyLoadedCartridge._title : "No cartridge";
        }


        public void TriggerTrophySelect(bool open) {
            _trophyMenu.SetActive(open);
            _homeMenu.SetActive(!open);
            EventSystem.current.SetSelectedGameObject(open ? _eventTrophiesTest : _eventHomeTest);
        }

        public void StartGame() {
            if (MicroController._instance.CurrentlyLoadedCartridge == null) return;
            MicroController._instance.Cartridge.StartGame();
            Activate(false);
        }
    }
}