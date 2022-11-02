using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pixygon.Micro {
    public class HomeController : MonoBehaviour {
        [SerializeField] private GameObject _homeMenu;
        [SerializeField] private GameObject _settingsMenu;
        [SerializeField] private GameObject _faceplateMenu;
        [SerializeField] private GameObject _cartridgeMenu;
        [SerializeField] private TextMeshProUGUI _versionText;
        [SerializeField] private GameObject _eventTest;

        public void Initialize() {
            GetComponent<Canvas>().worldCamera = MicroController._instance.Display._uiCamera;
            _versionText.text = MicroController._instance.Version;
        }
        public void Activate(bool activate) {
            _homeMenu.SetActive(activate);
            if(activate)
                EventSystem.current.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
        }
        
        public void TriggerSettingsMenu(bool open) {
            _settingsMenu.SetActive(open);
            _homeMenu.SetActive(!open);
            if(open)
                EventSystem.current.SetSelectedGameObject(_eventTest);
            else
                EventSystem.current.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
        }

        public void TriggerFaceplateSelect(bool open) {
            _faceplateMenu.SetActive(open);
            _homeMenu.SetActive(!open);
            if(open)
                EventSystem.current.SetSelectedGameObject(_eventTest);
            else
                EventSystem.current.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
        }
        public void SetFaceplate(int i) {
            PlayerPrefs.SetInt("Faceplate", i);
            PlayerPrefs.Save();
        }


        public void TriggerCartridgeSelect(bool open) {
            _cartridgeMenu.SetActive(open);
            _homeMenu.SetActive(!open);
            if(open)
                EventSystem.current.SetSelectedGameObject(_eventTest);
            else
                EventSystem.current.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
        }
        public void SetCartridge(int i) {
            PlayerPrefs.SetInt("Cartridge", i);
            PlayerPrefs.Save();
        }

        public void StartGame() {
            MicroController._instance.Cartridge.StartGame();
        }
    }
}