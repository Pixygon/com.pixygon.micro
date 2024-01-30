using Pixygon.Passport;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pixygon.Micro {
    public class AccountSignUp : MonoBehaviour {
        [SerializeField] private TMP_InputField _userInput;
        [SerializeField] private TMP_InputField _emailInput;
        [SerializeField] private TMP_InputField _passInput;
        
        [SerializeField] private GameObject _signupLoadingScreen;
        [SerializeField] private GameObject _signupErrorScreen;
        [SerializeField] private TextMeshProUGUI _signupErrorText;

        [SerializeField] private HomeAccountScreen _accountScreen;
        

        [SerializeField] private GameObject _usernameScreen;
        [SerializeField] private GameObject _emailScreen;
        [SerializeField] private GameObject _passwordScreen;
        [SerializeField] private GameObject _overviewScreen;
        
        [SerializeField] private GameObject _eventUsername;
        [SerializeField] private GameObject _eventEmail;
        [SerializeField] private GameObject _eventPassword;
        [SerializeField] private GameObject _eventLogin;
        [SerializeField] private GameObject _eventSignupFailed;
        
        public void StartSignup() {
            gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_eventUsername);
            _usernameScreen.SetActive(true);
            _emailScreen.SetActive(false);
            _passwordScreen.SetActive(false);
            _overviewScreen.SetActive(false);
        }
        
        public void UsernameEdited() {
            EventSystem.current.SetSelectedGameObject(_eventEmail);
            _usernameScreen.SetActive(false);
            _emailScreen.SetActive(true);
        }
        
        public void EmailEdited() {
            EventSystem.current.SetSelectedGameObject(_eventPassword);
            _emailScreen.SetActive(false);
            _passwordScreen.SetActive(true);
        }

        public void PasswordEdited() {
            EventSystem.current.SetSelectedGameObject(_eventLogin);
            _passwordScreen.SetActive(false);
            _overviewScreen.SetActive(true);
        }
        
        public void Signup() {
            MicroController._instance.Api.StartSignup(_userInput.text, _emailInput.text, _passInput.text, true, SignupComplete, SignupFailed);
            _signupLoadingScreen.SetActive(true);
        }

        private void SignupComplete() {
            if (!MicroController._instance.Api.IsLoggedIn) return;
            _accountScreen.SetAccountScreen();
            _signupLoadingScreen.SetActive(false);
            gameObject.SetActive(false);
            MicroController._instance.Home.HomeMainScreen.SetUsernameText();
        }
        
        private void SignupFailed(ErrorResponse error) {
            _signupLoadingScreen.SetActive(false);
            _signupErrorScreen.SetActive(true);
            _signupErrorText.text = $"Signup failed: {error._msg}";
            _signupLoadingScreen.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_eventSignupFailed);
            StartSignup();
        }
    }
}
