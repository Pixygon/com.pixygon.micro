using TMPro;
using UnityEngine;

namespace Pixygon.Micro {
    public class HomeController : MonoBehaviour {
        [SerializeField] private GameObject _homeMenu;
        [SerializeField] private GameObject _settingsMenu;
        [SerializeField] private GameObject _faceplateMenu;
        [SerializeField] private GameObject _cartridgeMenu;
        [SerializeField] private TextMeshProUGUI _versionText;

        public void Initialize() {
            GetComponent<Canvas>().worldCamera = MicroController._instance.Display._uiCamera;
            _versionText.text = MicroController._instance.Version;
        }
        public void Activate(bool activate) {
            _homeMenu.SetActive(activate);
        }
        
        public void TriggerSettingsMenu(bool open) {
            _settingsMenu.SetActive(open);
        }

        public void TriggerFaceplateSelect(bool open) {
            _faceplateMenu.SetActive(open);
        }
        public void SetFaceplate(int i) {
            PlayerPrefs.SetInt("Faceplate", i);
            PlayerPrefs.Save();
        }

        public void TriggerCartridgeSelect(bool open) {
            _cartridgeMenu.SetActive(open);
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