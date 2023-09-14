using Pixygon.DebugTool;
using Pixygon.NFT;
using Pixygon.Passport;
using Pixygon.Saving;
using UnityEngine;
using System;
using Pixygon.Core;
using Pixygon.Versioning;

namespace Pixygon.Micro {
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
        [SerializeField] private WalletFetcher _walletFetcher;
        [SerializeField] private PixygonApi _api;
        [SerializeField] private VersionData[] _versions;
        
        public bool HomeMenuOpen { get; private set; }
        public DisplayController Display { get; private set; }
        public InputController Input { get; private set; }
        public CartridgeController Cartridge { get; private set; }
        public ConsoleController Console { get; private set; }
        public HomeController Home { get; private set; }
        public bool SkipIntro => _skipIntro;
        public Cartridge[] Cartridges => _cartridges;
        public PixygonApi Api => _api;
        public Cartridge CurrentlyLoadedCartridge {
            get {
                if (PlayerPrefs.GetInt("Cartridge", -1) == -1)
                    return null;
                return _cartridges.Length != 0 ? _cartridges[PlayerPrefs.GetInt("Cartridge")] : null;
            }
        }
        public VersionData[] Versions => _versions;

        public string Version => _versions[_versions.Length-1].Version;

        private void Awake() {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
            Initialize();
        }
        private void Start() {
            UpdateVisualSettings();
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
            PauseManager.SetPause(HomeMenuOpen);
            Home.Activate(HomeMenuOpen);
        }
        public void CloseHomeMenu() {
            if (_cartridges.Length == 0) return;
            if (CurrentlyLoadedCartridge == null) return;
            if(!Api.IsLoggedIn) return;
            if(Cartridge.Game == null) return;
            HomeMenuOpen = false;
            PauseManager.SetPause(HomeMenuOpen);
        }
        public void SetCameraToDefault() {
            UpdateVisualSettings();
        }
        public void SetCameraToCartridgeSelect() {
            _cam.transform.position = new Vector3(0f, -8f, -20);
        }
        public void SetCameraToFaceplateSelect() {
            _cam.transform.position = new Vector3(0f, 0f, -20);
        }
        public void UpdateVisualSettings() {
            _cam.transform.position = new Vector3(0f, 0f, Mathf.Lerp(-20f, -7f, PlayerPrefs.GetFloat("Visual Zoom", .5f)));
            Console.transform.localEulerAngles = new Vector3(PlayerPrefs.GetFloat("Visual Yaw", 0f)*10f, PlayerPrefs.GetFloat("Visual Pitch", 0f)*10f, 0f);
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
                    _api.PatchMatWallet(wallet);
                    SaveManager.SettingsSave._user.matWallet = wallet;
                    break;
                case Chain.ImmutableX:
                    _api.PatchImxWallet(wallet);
                    SaveManager.SettingsSave._user.imxWallet = wallet;
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