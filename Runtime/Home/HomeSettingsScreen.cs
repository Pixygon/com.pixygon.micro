using UnityEngine;

namespace Pixygon.Micro {
    public class HomeSettingsScreen : MonoBehaviour {
        [SerializeField] private HomeController _homeScreen;
        [SerializeField] private SystemSettingsScreen _systemScreen;
        [SerializeField] private AudioSettingsScreen _audioScreen;
        [SerializeField] private DisplaySettingsScreen _displayScreen;
        [SerializeField] private GameObject _bindingsScreen;
        
        public void OpenScreen(bool open) {
            gameObject.SetActive(open);
            if (!open) {
                _homeScreen.TriggerAccountMenu(false);
                return;
            }
            _homeScreen.SelectSfx.Play();
            TriggerSystemScreen();
        }

        public void TriggerSystemScreen() {
            _systemScreen.OpenScreen(true);
            _audioScreen.OpenScreen(false);
            _displayScreen.OpenScreen(false);
        }
        public void TriggerAudioScreen() {
            _systemScreen.OpenScreen(false);
            _audioScreen.OpenScreen(true);
            _displayScreen.OpenScreen(false);
        }

        public void TriggerDisplayScreen() {
            _systemScreen.OpenScreen(false);
            _audioScreen.OpenScreen(false);
            _displayScreen.OpenScreen(true);
        }
        public void OpenDiscord() {
            Application.OpenURL("https://discord.gg/aNFfuYaxXP");
        }

        public void OpenX() {
            Application.OpenURL("https://twitter.com/Pixygon");
        }
        private void OnEnable() {
            MicroController._instance.Input._run += DoClose;
        }
        private void OnDisable() {
            MicroController._instance.Input._run -= DoClose;
        }
        public void OpenBindings(bool open) {
            _bindingsScreen.SetActive(open);
        }
        private void DoClose(bool started) {
            if (!started) return;
            _homeScreen.Activate(true);
            _homeScreen.BackSfx.Play();
        }
    }
}
