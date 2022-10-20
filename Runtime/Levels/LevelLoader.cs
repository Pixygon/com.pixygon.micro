using UnityEngine;

namespace Pixygon.Micro {
    public class LevelLoader : MonoBehaviour {
        [SerializeField] private LevelData[] _level;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Parallax _parallax;
        [SerializeField] private CameraController _camera;
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private GameObject _menuScreen;
        
        private Level _currentLevel;
        private GameObject _player;

        public GameObject GameOverScreen => _gameOverScreen;
        
        private void Start() {
            Initialize();
        }

        private void Initialize() {
            _menuScreen.SetActive(true);
        }

        public void StartLevel(int i) {
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
            _parallax.Initialize(_player.transform, level._parallaxLayerDatas);
            GetComponent<AudioSource>().clip = level._bgm;
            GetComponent<AudioSource>().Play();
            if(level._postProcessingProfile != null)
                MicroController._instance.Display._volume.profile = level._postProcessingProfile;
            else
                MicroController._instance.Display._volume.profile = MicroController._instance.Display._defaultVolume;
        }
    }
}