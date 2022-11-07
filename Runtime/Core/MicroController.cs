using Pixygon.DebugTool;
using UnityEngine;

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
        [SerializeField] private string _version;
        [SerializeField] private bool _skipIntro;
        [SerializeField] private Cartridge[] _cartridges;
        [SerializeField] private Camera _cam;
        
        public bool HomeMenuOpen { get; private set; }
        public DisplayController Display { get; private set; }
        public InputController Input { get; private set; }
        public CartridgeController Cartridge { get; private set; }
        public ConsoleController Console { get; private set; }
        public HomeController Home { get; private set; }
        public bool SkipIntro => _skipIntro;
        public string Version => _version;
        public Cartridge[] Cartridges => _cartridges;
        public Cartridge CurrentlyLoadedCartridge => _cartridges.Length != 0 ? _cartridges[PlayerPrefs.GetInt("Cartridge")] : null;
        private void Awake() {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
            Initialize();
        }
        private void Initialize() {
            Application.targetFrameRate = 60;
            Instantiate(_debuggerPrefab, transform);
            Display = Instantiate(_displayPrefab, transform);
            Input = Instantiate(_inputPrefab, transform);
            Console = Instantiate(_consolePrefab, transform);
            Cartridge = Instantiate(_cartridgePrefab, transform);
            Home = Instantiate(_homePrefab, transform);
            
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
                if(_cartridges.Length != 0)
                    HomeMenuOpen = false;
            }
            else {
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
    }
}