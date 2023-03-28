using System.Threading.Tasks;
using UnityEngine;

namespace Pixygon.Micro {
    public class FaceplateSelector : MonoBehaviour {
        [SerializeField] private FaceplateObject _faceplatePrefab;
        [SerializeField] private HomeController _home;
        
        private FaceplateObject[] _objects;
        private int _currentFaceplate;
        private FaceplateData[] _facePlates;
        private float _selectTimer;
        private bool _isClosing;
        
        public void Open() {
            MicroController._instance.SetCameraToFaceplateSelect();
            MicroController._instance.Input._move += Move;
            MicroController._instance.Input._jump += SelectFaceplate;
            MicroController._instance.Input._run += DoClose;
            MicroController._instance.Console.HideConsole(true);
            _currentFaceplate = PlayerPrefs.GetInt("Faceplate", 0);
            _facePlates = MicroController._instance.Console.Faceplates;
            _objects = new[] {
                Instantiate(_faceplatePrefab, new Vector3(-27f, 0f, 0f), Quaternion.identity),
                Instantiate(_faceplatePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity),
                Instantiate(_faceplatePrefab, new Vector3(27f, 0f, 0f), Quaternion.identity)
            };
            PopulateFaceplates();
        }

        public void Close() {
            _isClosing = true;
            MicroController._instance.Input._move -= Move;
            MicroController._instance.Input._jump -= SelectFaceplate;
            MicroController._instance.Input._run -= DoClose;
            MicroController._instance.SetCameraToDefault();
            MicroController._instance.Console.HideConsole(false);
            foreach (var g in _objects) {
                Destroy(g.gameObject);
            }
            ActuallyClose();
        }

        private async void ActuallyClose() {
            await Task.Yield();
            _objects = null;
            _home.TriggerCartridgeSelect(false);
            _isClosing = false;
        }

        public void PopulateFaceplates() {
            if (_currentFaceplate != 0) {
                _objects[0].gameObject.SetActive(true);
                _objects[0].Initialize(_facePlates[_currentFaceplate-1]);
            }
            else {
                _objects[0].gameObject.SetActive(false);
            }
            _objects[1].Initialize(_facePlates[_currentFaceplate]);

            if (_currentFaceplate != _facePlates.Length - 1) {
                _objects[2].gameObject.SetActive(true);
                _objects[2].Initialize(_facePlates[_currentFaceplate+1]);
            }
            else {
                _objects[2].gameObject.SetActive(false);
            }
        }

        public void Move(Vector2 v) {
            if (_selectTimer > 0f) return;
            if (v.x < -.5f) {
                if (_currentFaceplate == 0) return;
                _currentFaceplate -= 1;
            } else if (v.x > .5f) {
                if (_currentFaceplate == _facePlates.Length-1) return;
                _currentFaceplate += 1;
            }
            _selectTimer = .2f;
            PopulateFaceplates();
        }

        private void Update() {
            if(_selectTimer > 0f)
                _selectTimer -= Time.deltaTime;
        }
        private void DoClose(bool started) {
            if (!started) return;
            Close();
        }

        public void SelectFaceplate(bool started) {
            if (!started) return;
            if (!_objects[1].CanUse) return;
            if(_currentFaceplate > MicroController._instance.Faceplates.Length)
                _currentFaceplate = 0;
            PlayerPrefs.SetInt("Faceplate", _currentFaceplate);
            PlayerPrefs.Save();
            MicroController._instance.Console.UpdateFaceplate();
            Close();
        }
    }
}