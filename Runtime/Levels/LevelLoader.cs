using UnityEngine;

namespace Pixygon.Micro {
    public class LevelLoader : MonoBehaviour {
        [SerializeField] private Level _level;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Parallax _parallax;
        
        private Level _currentLevel;
        private GameObject _player;

        private void Start() {
            Initialize();
        }

        public void Initialize() {
            LoadLevel(_level);
        }

        public void ResetLevels() {
            _level.RespawnLevel();
        }

        public void LoadLevel(Level level) {
            if (_currentLevel != null)
                Destroy(_currentLevel.gameObject);
            _currentLevel = Instantiate(level, transform);
            _player = Instantiate(_playerPrefab, transform);
            _player.transform.position = _currentLevel.PlayerSpawn;
            GetComponent<CameraController>().Initialize(_player.transform);
            _player.GetComponent<MicroActor>().Initialize(this);
            _parallax.Initialize(_player.transform, level.ParallaxLayerDatas);
            //Spawn enemies
        }
    }
}