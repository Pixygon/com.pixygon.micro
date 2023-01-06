using Pixygon.DebugTool;
using Pixygon.Saving;
using UnityEngine;
using UnityEngine.Audio;

namespace Pixygon.Micro
{
    public class MicroController : MonoBehaviour {
        public static MicroController _instance;
        
        [SerializeField] private Debugger _debuggerPrefab;
        [SerializeField] private DisplayController _displayPrefab;
        [SerializeField] private InputController _inputPrefab;
        [SerializeField] private CartridgeController _cartridgePrefab;
        [SerializeField] private ConsoleController _consolePrefab;
        [SerializeField] private HomeController _homePrefab;
        [SerializeField] private SaveManager _saveManager;
        [SerializeField] private string _version;
        [SerializeField] private bool _skipIntro;
        [SerializeField] private Cartridge[] _cartridges;
        [SerializeField] private Faceplate[] _faceplates;
        [SerializeField] private Camera _cam;
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private WalletFetcher _walletFetcher;
        
        public bool HomeMenuOpen { get; private set; }
        public DisplayController Display { get; private set; }
        public InputController Input { get; private set; }
        public CartridgeController Cartridge { get; private set; }
        public ConsoleController Console { get; private set; }
        public HomeController Home { get; private set; }
        public string Wallet { get; private set; }
        public bool SkipIntro => _skipIntro;
        public string Version => _version;
        public Cartridge[] Cartridges => _cartridges;
        public Faceplate[] Faceplates => _faceplates;
        public Cartridge CurrentlyLoadedCartridge {
            get {
                if (PlayerPrefs.GetInt("Cartridge", -1) == -1)
                    return null;
                return _cartridges.Length != 0 ? _cartridges[PlayerPrefs.GetInt("Cartridge")] : null;
            }
        }
        public Faceplate CurrentlyLoadedFaceplate {
            get {
                if(PlayerPrefs.GetInt("Faceplate") >= _faceplates.Length)
                    PlayerPrefs.SetInt("Faceplate", 0);
                return _faceplates.Length != 0 ? _faceplates[PlayerPrefs.GetInt("Faceplate")] : null;
            }
        }

        private void Awake() {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
            Initialize();
            if(SaveManager.SettingsSave == null)
                SetWallet(string.Empty);
#if UNITY_EDITOR
            SetWallet("md1qw.wam");  
#endif
        }

        private void Start() {
            UpdateAudioSettings();
        }
        private void Initialize() {
            Application.targetFrameRate = 60;
            Instantiate(_debuggerPrefab, transform);
            Display = Instantiate(_displayPrefab, transform);
            Input = Instantiate(_inputPrefab, transform);
            Console = Instantiate(_consolePrefab, transform);
            Cartridge = Instantiate(_cartridgePrefab, transform);
            Home = Instantiate(_homePrefab, transform);
            Instantiate(_saveManager, transform);
            Cartridge.Initilize();
            Console.Initialize();
            Home.Initialize();
            Input._quit += Quit;
            Input._home += TriggerHomeMenu;
        }

        private void Quit(bool started) {
            #if !UNITY_WEBGL
            Application.Quit();
            #endif
        }

        public void TriggerHomeMenu(bool started) {
            if(!started) return;
            if (HomeMenuOpen) {
                if (_cartridges.Length != 0 && CurrentlyLoadedCartridge != null && Wallet != string.Empty && Cartridge.Game != null) {
                    Debug.Log("Home menu should close...");
                    HomeMenuOpen = false;
                }
            } else {
                HomeMenuOpen = true;
            }

            //This is a bad way to do it...
            //Time.timeScale = HomeMenuOpen ? 1f : 0f;
            PauseGame(HomeMenuOpen);
            Home.Activate(HomeMenuOpen);
        }

        private void PauseGame(bool pause) {
            foreach (var r in GetComponents<Rigidbody2D>()) {
                if (pause)
                    r.Sleep();
                else
                    r.WakeUp();
            }
        }

        public void SetZoom(float f) {
            _cam.transform.position = new Vector3(0f, 0f, Mathf.Lerp(-10f, -20f, f));
        }

        public void SetCameraToDefault() {
            _cam.transform.position = new Vector3(0f, 0f, -12);
        }
        public void SetCameraToCartridgeSelect() {
            _cam.transform.position = new Vector3(0f, -8f, -20);
        }
        public void SetCameraToFaceplateSelect() {
            _cam.transform.position = new Vector3(0f, 0f, -20);
        }
        public void UpdateAudioSettings() {
            _mixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume", 1f)) * 20f);
            _mixer.SetFloat("BGMVolume", Mathf.Log10(PlayerPrefs.GetFloat("BGMVolume", 1f)) * 20f);
            _mixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 1f)) * 20f);
        }

        public void GetWaxWallet() {
#if UNITY_WEBGL
            _walletFetcher.GetWaxAddress();
#endif
        }
        public void GetEthWallet() {
#if UNITY_WEBGL
            _walletFetcher.GetEthAddress();
#endif
        }
        public void GetTezWallet() {
#if UNITY_WEBGL
            _walletFetcher.GetTezAddress();
#endif
        }

        public void SetWallet(string wallet) {
            Wallet = wallet;
            if (SaveManager.SettingsSave == null)
                SaveManager.SettingsSave = new SettingsSaveData();
            SaveManager.SettingsSave._waxWallet = wallet;
            Home.SetWallet(wallet);
        }
    }
}