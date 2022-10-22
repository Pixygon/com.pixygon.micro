using UnityEngine;

namespace Pixygon.Micro {
    public class HomeController : MonoBehaviour {
        [SerializeField] private GameObject _settingsMenu;
        [SerializeField] private GameObject _faceplateMenu;
        [SerializeField] private GameObject _cartridgeMenu;
        
        
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
    }
}