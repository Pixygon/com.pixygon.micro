using Pixygon.Passport;
using UnityEngine;

namespace Pixygon.Micro {
    public class HomeController : MonoBehaviour {
        [SerializeField] private GameObject _homeMenu;
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _faceplateMenu;
        [SerializeField] private CartridgeSelector _cartridgeSelector;
        [SerializeField] private FaceplateSelector _faceplateSelector;
        [SerializeField] private HomeMainScreen _homeMainScreen;
        [SerializeField] private HomeAccountScreen _homeAccountScreen;
        [SerializeField] private HomeSettingsScreen _homeSettingsScreen;
        [SerializeField] private AccountUI _accountUI;
        [SerializeField] private AudioSource _selectSfx;
        [SerializeField] private AudioSource _backSfx;
        [SerializeField] private AudioSource _moveSfx;

        public AudioSource SelectSfx => _selectSfx;
        public AudioSource BackSfx => _backSfx;
        public AudioSource MoveSfx => _moveSfx;
        public HomeMainScreen HomeMainScreen => _homeMainScreen;
        
        private Cartridge _lastUsedCartridge;
        public void Initialize() {
            //GetComponent<Canvas>().worldCamera = MicroController._instance.Display._uiCamera;
            //GetComponent<Canvas>().sortingLayerName = "Menu";
            SetCurrentCartridge();
        }
        private void OnEnable() {
            _accountUI.OnLoginAction += () => { _homeMainScreen.PassportBadge.Set(); };
            _accountUI.OnLogoutAction += () => { _homeMainScreen.PassportBadge.Set(); };
        }
        private void OnDisable() {
            _accountUI.OnLoginAction -= () => { _homeMainScreen.PassportBadge.Set(); };
            _accountUI.OnLogoutAction -= () => { _homeMainScreen.PassportBadge.Set(); };
        }
        public void Activate(bool activate) {
            _homeMenu.SetActive(activate);
            _mainMenu.SetActive(activate);
            _homeMainScreen.Activate();
            _faceplateMenu.SetActive(false);
            _homeAccountScreen.gameObject.SetActive(false);
            _homeSettingsScreen.gameObject.SetActive(false);
            if(!activate)
                MicroController._instance.SetCameraToDefault();
            HomeMainScreen.Activate();
        }
        public void TriggerSettingsMenu(bool open) {
            _mainMenu.SetActive(!open);
            if(open)
            //    EventSystem.current.SetSelectedGameObject(_eventHomeTest);
            //else
                _homeSettingsScreen.OpenScreen(true);
        }
        public void TriggerAccountMenu(bool open) {
            _mainMenu.SetActive(!open);
            if(open)
                //EventSystem.current.SetSelectedGameObject(_eventHomeTest);
            //else
            _accountUI.StartLogin();
                //_homeAccountScreen.OpenScreen(true);
        }
        public void TriggerFaceplateSelect(bool open) {
            _mainMenu.SetActive(!open);
            _faceplateMenu.SetActive(open);
            if (open) _faceplateSelector.Open();
        }
        public void TriggerCartridgeSelect(bool open) {
            _mainMenu.SetActive(!open);
            //_cartridgeMenu.SetActive(open);
            if (open) _cartridgeSelector.Open();
        }
        public void SetCurrentCartridge() {
            var hasCartridge = MicroController._instance.CurrentlyLoadedCartridge != null;
            //_gameTitleText.text = hasCartridge ? MicroController._instance.CurrentlyLoadedCartridge._title : "No cartridge";
            //_gameBannerImage.sprite = hasCartridge ? MicroController._instance.CurrentlyLoadedCartridge._cartridgeBanner : _noGameSprite;
        }
        public void StartGame() {
            if (MicroController._instance.CurrentlyLoadedCartridge == null) return;
            if(_lastUsedCartridge != MicroController._instance.CurrentlyLoadedCartridge)
                MicroController._instance.Cartridge.StartGame();
            _lastUsedCartridge = MicroController._instance.CurrentlyLoadedCartridge;
            MicroController._instance.CloseHomeMenu();
            Activate(false);
        }
        public void WalletReceived() {
            _homeAccountScreen.WalletReceived();
        }
    }
}