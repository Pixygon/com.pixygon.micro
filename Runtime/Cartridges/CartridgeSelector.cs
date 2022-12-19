using UnityEngine;

namespace Pixygon.Micro
{
    public class CartridgeSelector : MonoBehaviour {
        [SerializeField] private CartridgeObject _cartridgePrefab;
        
        private CartridgeObject[] _objects;
        private int _currentCartridge;
        private Cartridge[] _cartridges;
        private float _selectTimer;
        
        public void Open() {
            MicroController._instance.SetCameraToCartridgeSelect();
            MicroController._instance.Input._move += Move;
            MicroController._instance.Input._jump += SelectCartridge;
            _currentCartridge = 0;
            _cartridges = MicroController._instance.Cartridges;
            _objects = new[] {
                Instantiate(_cartridgePrefab, new Vector3(-18f, -12f, 0f), Quaternion.identity),
                Instantiate(_cartridgePrefab, new Vector3(0f, -12f, 0f), Quaternion.identity),
                Instantiate(_cartridgePrefab, new Vector3(18f, -12f, 0f), Quaternion.identity)
            };
            PopulateCartridges();
        }

        public void Close() {
            MicroController._instance.Input._move -= Move;
            MicroController._instance.Input._jump -= SelectCartridge;
            MicroController._instance.SetCameraToDefault();
            foreach (var g in _objects) {
                Destroy(g.gameObject);
            }
            _objects = null;
        }

        public void PopulateCartridges() {
            if (_currentCartridge != 0) {
                _objects[0].gameObject.SetActive(true);
                _objects[0].Initialize(_cartridges[_currentCartridge-1]);
            }
            else {
                _objects[0].gameObject.SetActive(false);
            }
            _objects[1].Initialize(_cartridges[_currentCartridge]);

            if (_currentCartridge != _cartridges.Length - 1) {
                _objects[2].gameObject.SetActive(true);
                _objects[2].Initialize(_cartridges[_currentCartridge+1]);
            }
            else {
                _objects[2].gameObject.SetActive(false);
            }
        }

        public void Move(Vector2 v) {
            if (_selectTimer > 0f) return;
            if (v.x < -.5f) {
                if (_currentCartridge == 0) return;
                _currentCartridge -= 1;
            } else if (v.x > .5f) {
                if (_currentCartridge == _cartridges.Length-1) return;
                _currentCartridge += 1;
            }
            _selectTimer = .2f;
            PopulateCartridges();
        }

        private void Update() {
            if(_selectTimer > 0f)
                _selectTimer -= Time.deltaTime;
        }

        [SerializeField] private HomeController _home;
        public void SelectCartridge(bool started) {
            if (!started) return;
            if(_currentCartridge > MicroController._instance.Cartridges.Length)
                _currentCartridge = 0;
            PlayerPrefs.SetInt("Cartridge", _currentCartridge);
            PlayerPrefs.Save();
            _home.SetCurrentCartridge();
            Close();
        }
    }
}
