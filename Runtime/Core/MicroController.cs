using UnityEngine;

namespace Pixygon.Micro
{
    public class MicroController : MonoBehaviour {
        public static MicroController _instance;
        
        [SerializeField] private DisplayController _displayPrefab;
        [SerializeField] private InputController _inputPrefab;
        [SerializeField] private CartridgeController _cartridgePrefab;
        [SerializeField] private ConsoleController _consolePrefab;
        [SerializeField] private string _version;
        [SerializeField] private bool _skipIntro;
        [SerializeField] private Cartridge[] _cartridges;
        
        public DisplayController Display { get; private set; }
        public InputController Input { get; private set; }
        public CartridgeController Cartridge { get; private set; }
        public ConsoleController Console { get; private set; }
        public bool SkipIntro => _skipIntro;
        public string Version => _version;
        public Cartridge[] Cartridges => _cartridges;
        
        private void Awake() {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
            Initialize();
        }
        private void Initialize() {
            Application.targetFrameRate = 60;
            Display = Instantiate(_displayPrefab, transform);
            Input = Instantiate(_inputPrefab, transform);
            Cartridge = Instantiate(_cartridgePrefab, transform);
            Console = Instantiate(_consolePrefab, transform);
            
            Cartridge.Initilize();
            Console.Initialize();
            Input._quit += Quit;
        }

        private void Quit(bool started) {
            #if !UNITY_WEBGL
            Application.Quit();
            #endif
        }
    }
}