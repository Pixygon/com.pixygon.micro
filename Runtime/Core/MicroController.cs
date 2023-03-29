using Pixygon.DebugTool;
using Pixygon.NFT;
using Pixygon.Passport;
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
        [SerializeField] private Camera _cam;
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private WalletFetcher _walletFetcher;
        [SerializeField] private PixygonApi _api;
        
        public bool HomeMenuOpen { get; private set; }
        public DisplayController Display { get; private set; }
        public InputController Input { get; private set; }
        public CartridgeController Cartridge { get; private set; }
        public ConsoleController Console { get; private set; }
        public HomeController Home { get; private set; }
        public bool SkipIntro => _skipIntro;
        public string Version => _version;
        public Cartridge[] Cartridges => _cartridges;
        public PixygonApi Api => _api;
        
        public Cartridge CurrentlyLoadedCartridge {
            get {
                if (PlayerPrefs.GetInt("Cartridge", -1) == -1)
                    return null;
                return _cartridges.Length != 0 ? _cartridges[PlayerPrefs.GetInt("Cartridge")] : null;
            }
        }
        private void Awake() {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
            Initialize();
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
            Input._home += OpenHomeMenu;
        }

        public void OpenHomeMenu(bool started) {
            if (!started || HomeMenuOpen) return;
            HomeMenuOpen = true;
            PauseGame(HomeMenuOpen);
            Home.Activate(HomeMenuOpen);
        }
        public void CloseHomeMenu() {
            if (_cartridges.Length == 0) return;
            if (CurrentlyLoadedCartridge == null) return;
            if(!Api.IsLoggedIn) return;
            if(Cartridge.Game == null) return;
            HomeMenuOpen = false;
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
        public void GetWallet(Chain chain, int walletProvider) {
            _walletFetcher.GetWallet(chain, walletProvider, SetWallet);
        }
        public void SetWallet(Chain chain, string wallet) {
            switch (chain) {
                case Chain.Wax:
                    _api.PatchWaxWallet(wallet);
                    SaveManager.SettingsSave._user.waxWallet = wallet;
                    break;
                case Chain.EOS:
                    break;
                case Chain.Ethereum:
                    _api.PatchEthWallet(wallet);
                    SaveManager.SettingsSave._user.ethWallet = wallet;
                    break;
                case Chain.Tezos:
                    _api.PatchTezWallet(wallet);
                    SaveManager.SettingsSave._user.tezWallet = wallet;
                    break;
                case Chain.Polygon:
                    break;
                case Chain.Solana:
                    break;
                case Chain.Flow:
                    break;
            }
            Home.WalletReceived();
        }
    }
}