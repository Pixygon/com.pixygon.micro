using Pixygon.DebugTool;
using UnityEngine;

namespace Pixygon.Micro {
    public class LevelLoader : MonoBehaviour {
        [SerializeField] private LevelData[] _level;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Parallax _parallaxPrefab;
        [SerializeField] private CameraController _camera;
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private GameObject _menuScreen;
        
        private Level _currentLevel;
        private GameObject _player;
        private Parallax _parallax;
        private bool _levelLoaded;

        public GameObject GameOverScreen => _gameOverScreen;
        
        private void Start() {
            Initialize();
        }

        private void OnEnable() {
            MicroController._instance.Input._jump += SelectLevel;
        }

        private void OnDisable() {
            MicroController._instance.Input._jump -= SelectLevel;
        }

        private void Initialize() {
            Log.DebugMessage(DebugGroup.PixygonMicro, "Game started", this);
            _menuScreen.SetActive(true);
        }

        private void SelectLevel(bool started) {
            if (!started) return;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Selected level", this);
            if (!_levelLoaded) return;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Level was not loaded...", this);
            StartLevel(0);
        }

        public void StartLevel(int i) {
            Log.DebugMessage(DebugGroup.PixygonMicro, "Select Level: " + i, this);
            _menuScreen.SetActive(false);
            LoadLevel(_level[i]);
        }

        public void ResetLevels() {
            _currentLevel.RespawnLevel();
        }

        public void LoadLevel(LevelData level) {
            if (_currentLevel != null)
                Destroy(_currentLevel.gameObject);
            _currentLevel = Instantiate(level._levelPrefab, transform).GetComponent<Level>();
            _player = Instantiate(_playerPrefab, transform);
            _player.transform.position = _currentLevel.PlayerSpawn;
            _camera.Initialize(_player.transform);
            _player.GetComponent<MicroActor>().Initialize(this);
            _parallax = Instantiate(_parallaxPrefab, transform);
            _parallax.Initialize(_player.transform, level._parallaxLayerDatas);
            GetComponent<AudioSource>().clip = level._bgm;
            GetComponent<AudioSource>().Play();
            if(level._postProcessingProfile != null)
                MicroController._instance.Display._volume.profile = level._postProcessingProfile;
            else
                MicroController._instance.Display._volume.profile = MicroController._instance.Display._defaultVolume;
            _levelLoaded = true;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Level loaded!", this);
        }
    }
}