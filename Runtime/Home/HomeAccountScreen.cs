using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pixygon.Micro {
    public class HomeAccountScreen : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _usernameText;
        
        [SerializeField] private GameObject _accountUserPage;
        
        [SerializeField] private GameObject _eventAccount;
        [SerializeField] private GameObject _eventNoAccount;

        [SerializeField] private GameObject _waxIcon;
        [SerializeField] private GameObject _ethIcon;
        [SerializeField] private GameObject _tezIcon;

        [SerializeField] private HomeController _homeScreen;
        [SerializeField] private AccountLogin _accountLogin;
        [SerializeField] private AccountSignUp _accountSignup;
        [SerializeField] private AccountWallet _accountWallet;

        [SerializeField] private GameObject _noAccountScreen;
        
        public void Logout() {
            MicroController._instance.Api.StartLogout();
            SetAccountScreen();
            MicroController._instance.Home.BackSfx.Play();
        }

        public void StartLogin() {
            _accountLogin.StartLogin();
            _noAccountScreen.SetActive(false);
            MicroController._instance.Home.SelectSfx.Play();
        }

        public void StartSignup() {
            _accountSignup.StartSignup();
            _noAccountScreen.SetActive(false);
            MicroController._instance.Home.SelectSfx.Play();
        }

        public void StartWalletFetch() {
            _accountWallet.OpenWalletScreen();
            MicroController._instance.Home.SelectSfx.Play();
        }

        [SerializeField] private GameObject _walletSelectionButton;

        public void SetAccountScreen() {
            if (!MicroController._instance.Api.IsLoggedIn) {
                _accountUserPage.SetActive(false);
                _noAccountScreen.SetActive(true);
                EventSystem.current.SetSelectedGameObject(_eventNoAccount);
                MicroController._instance.Home.HomeMainScreen.SetUsernameText();
            } else {
                _accountUserPage.SetActive(true);
                EventSystem.current.SetSelectedGameObject(_eventAccount);
                _waxIcon.SetActive(!string.IsNullOrWhiteSpace(Saving.SaveManager.SettingsSave._user.waxWallet));
                _ethIcon.SetActive(!string.IsNullOrWhiteSpace(Saving.SaveManager.SettingsSave._user.ethWallet));
                _tezIcon.SetActive(!string.IsNullOrWhiteSpace(Saving.SaveManager.SettingsSave._user.tezWallet));
                _usernameText.text = Saving.SaveManager.SettingsSave._user.userName;
                #if UNITY_IOS || UNITY_ANDROID
                _walletSelectionButton.SetActive(false);
                #endif
            }
        }
        public void OpenScreen(bool open) {
            gameObject.SetActive(open);
            if (!open) {
                _homeScreen.TriggerAccountMenu(false);
                return;
            }
            MicroController._instance.Home.SelectSfx.Play();
            //EventSystem.current.SetSelectedGameObject(_eventLogin);
            SetAccountScreen();
        }


        public void WalletReceived() {
            _accountWallet.WalletReceived();
        }
    }
}