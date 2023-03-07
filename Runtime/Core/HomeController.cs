using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pixygon.Micro {
    public class HomeController : MonoBehaviour {
        [SerializeField] private GameObject _homeMenu;
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _faceplateMenu;
        [SerializeField] private GameObject _cartridgeMenu;
        [SerializeField] private TextMeshProUGUI _gameTitleText;
        [SerializeField] private TextMeshProUGUI _usernameText;
        [SerializeField] private GameObject _eventHomeTest;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private CartridgeSelector _cartridgeSelector;
        [SerializeField] private FaceplateSelector _faceplateSelector;
        
        [SerializeField] private HomeAccountScreen _homeAccountScreen;
        [SerializeField] private HomeSettingsScreen _homeSettingsScreen;

        public void Initialize() {
            GetComponent<Canvas>().worldCamera = MicroController._instance.Display._uiCamera;
            GetComponent<Canvas>().sortingLayerName = "Menu";
            SetCurrentCartridge();
        }
        public void Activate(bool activate) {
            _homeMenu.SetActive(activate);
            _mainMenu.SetActive(activate);
            _cartridgeMenu.SetActive(false);
            _faceplateMenu.SetActive(false);
            _homeAccountScreen.gameObject.SetActive(false);
            _homeSettingsScreen.gameObject.SetActive(false);
            if(activate)
                EventSystem.current.SetSelectedGameObject(_eventHomeTest);
            else
                MicroController._instance.SetCameraToDefault();
        }
        public void TriggerSettingsMenu(bool open) {
            _mainMenu.SetActive(!open);
            if(!open)
                EventSystem.current.SetSelectedGameObject(_eventHomeTest);
            else
                _homeSettingsScreen.OpenScreen(true);
        }
        public void TriggerAccountMenu(bool open) {
            _mainMenu.SetActive(!open);
            if(!open)
                EventSystem.current.SetSelectedGameObject(_eventHomeTest);
            else
                _homeAccountScreen.OpenScreen(true);
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
        public void StartGame() {
            if (MicroController._instance.CurrentlyLoadedCartridge == null) return;
            MicroController._instance.Cartridge.StartGame();
            MicroController._instance.CloseHomeMenu();
            Activate(false);
        }
    }
}