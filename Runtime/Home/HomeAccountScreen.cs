using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pixygon.Micro {
    public class HomeAccountScreen : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _usernameText;
        [SerializeField] private TextMeshProUGUI _waxWalletText;
        [SerializeField] private TextMeshProUGUI _ethWalletText;
        [SerializeField] private TextMeshProUGUI _tezWalletText;
        
        [SerializeField] private GameObject _accountUserPage;
        [SerializeField] private GameObject _accountWalletPage;
        
        [SerializeField] private GameObject _eventAccount;
        [SerializeField] private GameObject _eventWallet;
        [SerializeField] private GameObject _eventNoAccount;

        [SerializeField] private GameObject _waxIcon;
        [SerializeField] private GameObject _ethIcon;
        [SerializeField] private GameObject _tezIcon;
        
        [SerializeField] private GameObject _waxCheck;
        [SerializeField] private GameObject _ethCheck;
        [SerializeField] private GameObject _tezCheck;

        [SerializeField] private HomeController _homeScreen;
        [SerializeField] private AccountLogin _accountLogin;
        [SerializeField] private AccountSignUp _accountSignup;

        [SerializeField] private GameObject _noAccountScreen;
        
        [SerializeField] private GameObject _walletLoadingScreen;

        public void GetWaxWallet() {
            MicroController._instance.GetWaxWallet();
            _walletLoadingScreen.SetActive(true);
        }
        public void GetEthWallet() {
            MicroController._instance.GetEthWallet();
            _walletLoadingScreen.SetActive(true);
        }
        public void GetTezWallet() {
            MicroController._instance.GetTezWallet();
            _walletLoadingScreen.SetActive(true);
        }
        public void SetWaxWallet(string s) {
            _walletLoadingScreen.SetActive(false);
        }


        
        public void Logout() {
            MicroController._instance.Api.StartLogout();
            SetAccountScreen();
        }

        public void StartLogin() {
            _accountLogin.StartLogin();
            _noAccountScreen.SetActive(false);
        }

        public void StartSignup() {
            _accountSignup.StartSignup();
            _noAccountScreen.SetActive(false);
        }

        public void SetAccountScreen() {
            if (!MicroController._instance.Api.IsLoggedIn) {
                _accountUserPage.SetActive(false);
                _noAccountScreen.SetActive(true);
                EventSystem.current.SetSelectedGameObject(_eventNoAccount);
            } else {
                _accountUserPage.SetActive(true);
                EventSystem.current.SetSelectedGameObject(_eventAccount);
                _waxIcon.SetActive(Saving.SaveManager.SettingsSave._user.waxWallet != string.Empty);
                _ethIcon.SetActive(Saving.SaveManager.SettingsSave._user.ethWallet != string.Empty);
                _tezIcon.SetActive(Saving.SaveManager.SettingsSave._user.tezWallet != string.Empty);
                _usernameText.text = Saving.SaveManager.SettingsSave._user.userName;
            }
        }
        public void OpenScreen(bool open) {
            gameObject.SetActive(open);
            if (!open) {
                _homeScreen.TriggerAccountMenu(false);
                return;
            }
            //EventSystem.current.SetSelectedGameObject(_eventLogin);
            SetAccountScreen();
        }
        public void OpenWalletScreen(bool open) {
            _accountWalletPage.SetActive(open);
            EventSystem.current.SetSelectedGameObject(open ? _eventWallet : _eventAccount);
            if (!open) return;
            _waxCheck.SetActive(!string.IsNullOrWhiteSpace(Saving.SaveManager.SettingsSave._user.waxWallet));
            _ethCheck.SetActive(!string.IsNullOrWhiteSpace(Saving.SaveManager.SettingsSave._user.ethWallet));
            _tezCheck.SetActive(!string.IsNullOrWhiteSpace(Saving.SaveManager.SettingsSave._user.tezWallet));
            _waxWalletText.text = Saving.SaveManager.SettingsSave._user.waxWallet;
            _ethWalletText.text = Saving.SaveManager.SettingsSave._user.ethWallet;
            _tezWalletText.text = Saving.SaveManager.SettingsSave._user.tezWallet;
        }

        public void WalletReceived() {
            _walletLoadingScreen.SetActive(false);
        }
    }
}