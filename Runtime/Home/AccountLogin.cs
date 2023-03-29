using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pixygon.Micro {
    public class AccountLogin : MonoBehaviour {
        [SerializeField] private TMP_InputField _userInput;
        [SerializeField] private TMP_InputField _passInput;
        
        [SerializeField] private GameObject _loginLoadingScreen;
        [SerializeField] private GameObject _loginErrorScreen;
        [SerializeField] private TextMeshProUGUI _loginErrorText;

        [SerializeField] private HomeAccountScreen _accountScreen;
        

        [SerializeField] private GameObject _usernameScreen;
        [SerializeField] private GameObject _passwordScreen;
        [SerializeField] private GameObject _overviewScreen;
        
        [SerializeField] private GameObject _eventUsername;
        [SerializeField] private GameObject _eventPassword;
        [SerializeField] private GameObject _eventLogin;
        [SerializeField] private GameObject _eventLoginFailed;
        
        public void StartLogin() {
            gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_eventUsername);
            _usernameScreen.SetActive(true);
            _passwordScreen.SetActive(false);
            _overviewScreen.SetActive(false);
            MicroController._instance.Home.SelectSfx.Play();
        }
        
        public void UsernameEdited() {
            EventSystem.current.SetSelectedGameObject(_eventPassword);
            _usernameScreen.SetActive(false);
            _passwordScreen.SetActive(true);
            MicroController._instance.Home.SelectSfx.Play();
        }

        public void PasswordEdited() {
            EventSystem.current.SetSelectedGameObject(_eventLogin);
            _passwordScreen.SetActive(false);
            _overviewScreen.SetActive(true);
            MicroController._instance.Home.SelectSfx.Play();
        }
        
        public void Login() {
            MicroController._instance.Api.StartLogin(_userInput.text, _passInput.text, true, LoginComplete, LoginFailed);
            _loginLoadingScreen.SetActive(true);
            MicroController._instance.Home.SelectSfx.Play();
        }

        private void LoginComplete() {
            if (!MicroController._instance.Api.IsLoggedIn) return;
            _accountScreen.SetAccountScreen();
            _loginLoadingScreen.SetActive(false);
            gameObject.SetActive(false);
            MicroController._instance.Home.SetUsernameText();
            MicroController._instance.Home.SelectSfx.Play();
        }
        
        private void LoginFailed(string s) {
            _loginErrorScreen.SetActive(true);
            _loginErrorText.text = $"Login failed: {s}";
            _loginLoadingScreen.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_eventLoginFailed);
            MicroController._instance.Home.SelectSfx.Play();
        }

        public void CloseFailedLogin() {
            _loginErrorScreen.SetActive(false);
            _loginErrorText.text = "";
            StartLogin();
            MicroController._instance.Home.BackSfx.Play();
        }
    }
}
