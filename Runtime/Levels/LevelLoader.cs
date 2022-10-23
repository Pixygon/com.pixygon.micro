using System.Threading.Tasks;
using Pixygon.Addressable;
using Pixygon.DebugTool;
using UnityEngine;
using UnityEngine.Rendering;

namespace Pixygon.Micro {
    public class LevelLoader : MonoBehaviour {
        [SerializeField] private LevelData[] _level;
        [SerializeField] private MicroActorData _playerData;
        [SerializeField] private Parallax _parallaxPrefab;
        [SerializeField] private CameraController _camera;
        [SerializeField] private UI _ui;
        private Level _currentLevel;
        private GameObject _player;
        private Parallax _parallax;
        private bool _levelLoaded;

        public UI Ui => _ui;
        
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
            //Ui.Initialize();
            Ui.TriggerMenuScreen(true);
        }

        private void SelectLevel(bool started) {
            if (!started) return;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Selected level", this);
            if (_levelLoaded) return;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Level was not loaded...", this);
            StartLevel(0);
        }

        public void StartLevel(int i) {
            Log.DebugMessage(DebugGroup.PixygonMicro, "Select Level: " + i, this);
            Ui.TriggerMenuScreen(false);
            LoadLevel(_level[i]);
        }

        public void ResetLevels() {
            _currentLevel.RespawnLevel(this);
        }

        private LevelData _currentLevelData;

        public async void LoadLevel(LevelData level) {
            _currentLevelData = level;
            await SetupLevel();
            await SetupPlayer();
            await SetupParallax();
            await SetupBgm();
            await SetupPostProc();
            _levelLoaded = true;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Level loaded!", this);
        }

        private async Task SetupLevel() {
            if (_currentLevel != null)
                Destroy(_currentLevel.gameObject);
            
            var g = await AddressableLoader.LoadGameObject(_currentLevelData._levelRef, transform);
            _currentLevel = g.GetComponent<Level>();
            //_currentLevel = Instantiate(_currentLevelData._levelPrefab, transform).GetComponent<Level>();
            _currentLevel.RespawnLevel(this);
        }
        private async Task SetupPlayer() {
            _player = await AddressableLoader.LoadGameObject(_playerData._actorRef, transform);
            _player.transform.position = _currentLevel.PlayerSpawn;
            _camera.Initialize(_player.transform);
            _player.GetComponent<MicroActor>().Initialize(this, _playerData);
        }
        private async Task SetupParallax() {
            _parallax = Instantiate(_parallaxPrefab, transform);
            _parallax.Initialize(_player.transform, _currentLevelData._parallaxLayerDatas);
        }
        private async Task SetupBgm() {
            GetComponent<AudioSource>().clip = await AddressableLoader.LoadAsset<AudioClip>(_currentLevelData._bgmRef);
            GetComponent<AudioSource>().Play();
        }
        private async Task SetupPostProc() {
            MicroController._instance.Display._volume.profile = _currentLevelData._postProcessingProfileRef != null ?
                await AddressableLoader.LoadAsset<VolumeProfile>(_currentLevelData._postProcessingProfileRef) : MicroController._instance.Display._defaultVolume;
        }
    }
}