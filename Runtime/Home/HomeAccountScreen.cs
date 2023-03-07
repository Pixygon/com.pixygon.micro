using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pixygon.Micro {
    public class HomeAccountScreen : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _usernameText;
        [SerializeField] private TextMeshProUGUI _waxWalletText;
        [SerializeField] private TextMeshProUGUI _ethWalletText;
        [SerializeField] private TextMeshProUGUI _tezWalletText;
        
        [SerializeField] private TMP_InputField _userInput;
        [SerializeField] private TMP_InputField _passInput;
        [SerializeField] private GameObject _accountLoginPage;
        [SerializeField] private GameObject _accountUserPage;
        [SerializeField] private GameObject _accountWalletPage;
        
        [SerializeField] private GameObject _eventLogin;
        [SerializeField] private GameObject _eventAccount;
        [SerializeField] private GameObject _eventWallet;

        [SerializeField] private GameObject _waxIcon;
        [SerializeField] private GameObject _ethIcon;
        [SerializeField] private GameObject _tezIcon;
        
        [SerializeField] private GameObject _waxCheck;
        [SerializeField] private GameObject _ethCheck;
        [SerializeField] private GameObject _tezCheck;

        [SerializeField] private HomeController _homeScreen;

        public void GetWaxWallet() {
            MicroController._instance.GetWaxWallet();
        }
        public void GetEthWallet() {
            MicroController._instance.GetEthWallet();
        }
        public void GetTezWallet() {
            MicroController._instance.GetTezWallet();
        }
        public void SetWaxWallet(string s) {
            _waxWalletText.text = s;
        }
        public void Login() {
            MicroController._instance.Api.StartLogin(_userInput.text, _passInput.text, true, SetAccountScreen);
        }
        public void Logout() {
            MicroController._instance.Api.StartLogout();
            SetAccountScreen();
        }
        public void SetAccountScreen() {
            if (!MicroController._instance.Api.IsLoggedIn) {
                _accountLoginPage.SetActive(true);
                _accountUserPage.SetActive(false);
                EventSystem.current.SetSelectedGameObject(_eventLogin);
                //SetWallet(MicroController._instance.Api.AccountData.user.waxWallet);
            } else {
                //SetWallet(MicroController._instance.Api.AccountData.user.userName);
                _accountLoginPage.SetActive(false);
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
            EventSystem.current.SetSelectedGameObject(_eventLogin);
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
    }
}