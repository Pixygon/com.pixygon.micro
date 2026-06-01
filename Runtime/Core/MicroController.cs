using System.Collections.Generic;
using Pixygon.DebugTool;
using Pixygon.NFT;
using Pixygon.Passport;
using Pixygon.Saving;
using UnityEngine;
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
            
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
            var newCarts = new List<Cartridge>();
            foreach (var c in Cartridges) {
                if (!c._testingCartridge)
                    newCarts.Add(c);
            }
            _cartridges = newCarts.ToArray();
#endif
            Application.targetFrameRate = 60;
            Instantiate(_debuggerPrefab, transform);
            Display = Instantiate(_displayPrefab, transform);
            Input = Instantiate(_inputPrefab, transform);
            Console = Instantiate(_consolePrefab, transform);
            Cartridge = Instantiate(_cartridgePrefab, transform);
            Home = Instantiate(_homePrefab, Console.ScreenCanvas);
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
        // Pixygon NFT wallet linking was removed: WalletFetcher is now a no-op
        // stub (see com.pixygon.passport) that never invokes the callback, so
        // the old SetWallet handler is dead and has been deleted. GetWallet is
        // kept as a thin stub call so AccountWallet's buttons still compile and
        // bind; clicking one just logs the WalletFetcher's one-shot warning.
        public void GetWallet(Chain chain, int walletProvider) {
            _walletFetcher.GetWallet((int)chain, walletProvider, null);
        }
    }
}