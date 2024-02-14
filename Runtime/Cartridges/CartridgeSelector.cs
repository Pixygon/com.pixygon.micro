using System.Threading.Tasks;
using UnityEngine;

namespace Pixygon.Micro {
    public class CartridgeSelector : MonoBehaviour {
        [SerializeField] private CartridgeObject _cartridgePrefab;
        [SerializeField] private HomeController _home;
        
        private CartridgeObject[] _objects;
        private int _currentCartridge;
        private Cartridge[] _cartridges;
        private float _selectTimer;
        private bool _isClosing;
        
        public void Open() {
            MicroController._instance.SetCameraToCartridgeSelect();
            MicroController._instance.Input._move += Move;
            MicroController._instance.Input._jump += SelectCartridge;
            MicroController._instance.Input._run += DoClose;
            _currentCartridge = PlayerPrefs.GetInt("Cartridge", 0);
            if (_currentCartridge == -1)
                _currentCartridge = 0;
            _cartridges = MicroController._instance.Cartridges;
            _home.SelectSfx.Play();
            ClearCartridges();
            PopulateCartridges();
        }
        private void Close() {
            _isClosing = true;
            MicroController._instance.Input._move -= Move;
            MicroController._instance.Input._jump -= SelectCartridge;
            MicroController._instance.Input._run -= DoClose;
            MicroController._instance.SetCameraToDefault();
            ClearCartridges();
            ActuallyClose();
        }
        private async void ActuallyClose() {
            await Task.Yield();
            _objects = null;
            _isClosing = false;
        }
        private void PopulateCartridges() {
            ClearCartridges();
            _objects = new[] {
                Instantiate(_cartridgePrefab, new Vector3(-18f, -12f, 0f), Quaternion.identity),
                Instantiate(_cartridgePrefab, new Vector3(0f, -12f, 0f), Quaternion.identity),
                Instantiate(_cartridgePrefab, new Vector3(18f, -12f, 0f), Quaternion.identity)
            };
            RefreshCartridges();
        }
        private void ClearCartridges() {
            if (_objects == null) return;
            foreach (var g in _objects) {
                Destroy(g.gameObject);
            }
        }
        private void RefreshCartridges() {
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
        private void Move(Vector2 v) {
            if (_selectTimer > 0f) return;
            switch (v.x) {
                case < -.5f when _currentCartridge == 0:
                    _currentCartridge = _cartridges.Length-1;
                    break;
                case < -.5f:
                    _currentCartridge -= 1;
                    break;
                case > .5f when _currentCartridge == _cartridges.Length-1:
                    _currentCartridge = 0;
                    break;
                case > .5f:
                    _currentCartridge += 1;
                    break;
            }
            _selectTimer = .2f;
            _home.MoveSfx.Play();
            RefreshCartridges();
        }
        private void Update() {
            if(_selectTimer > 0f)
                _selectTimer -= Time.deltaTime;
        }
        public void DoClose(bool started) {
            if (!started) return;
            _home.BackSfx.Play();
            Close();
        }
        public void SelectCartridge(bool started) {
            if (!started) return;
            if (!_objects[1].CanUse) return;
            if(_currentCartridge > MicroController._instance.Cartridges.Length)
                _currentCartridge = 0;
            PlayerPrefs.SetInt("Cartridge", _currentCartridge);
            PlayerPrefs.Save();
            _home.SetCurrentCartridge();
            Close();
        }
    }
}
