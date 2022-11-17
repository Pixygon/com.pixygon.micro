using System.Threading.Tasks;
using Pixygon.Addressable;
using Pixygon.DebugTool;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;

namespace Pixygon.Micro {
    public class LevelLoader : MonoBehaviour {
        [SerializeField] private LevelData[] _level;
        [SerializeField] private MicroActorData _playerData;
        [SerializeField] private AssetReference _parallaxPrefabRef;
        [SerializeField] private CameraController _camera;
        [SerializeField] private UI _ui;
        
        private GameObject _player;
        private Parallax.Parallax _parallax;
        private bool _levelLoaded;
        private int _currentLevelId;

        public UI Ui => _ui;
        public CameraController Camera => _camera;
        public Level CurrentLevel { get; private set; }
        public LevelData CurrentLevelData { get; private set; }
        
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
            if (_levelLoaded || _loadingLevel) return;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Level was not loaded...", this);
            StartLevel(_currentLevelId);
        }

        public void StartLevel(int i) {
            _loadingLevel = true;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Select Level: " + i, this);
            Ui.TriggerMenuScreen(false);
            LoadLevel(_level[i]);
        }

        private bool _loadingLevel;

        public void EndLevel() {
            if (!_levelLoaded) return;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Ending level!", this);
            _levelLoaded = false;
            if (_currentLevelId < _level.Length) {
                _currentLevelId += 1;
                StartLevel(_currentLevelId);
            }
            else {
                Log.DebugMessage(DebugGroup.PixygonMicro, "Game completed!!");
                _currentLevelId = 0;
                StartLevel(_currentLevelId);
            }
        }
        public void ResetLevels() {
            CurrentLevel.RespawnLevel(this);
        }
        private async void LoadLevel(LevelData level) {
            if (_levelLoaded) return;
            _levelLoaded = false;
            CurrentLevelData = level;
            await SetupLevel();
            await SetupPlayer();
            await SetupParallax();
            await SetupBgm();
            await SetupPostProc();
            _levelLoaded = true;
            _loadingLevel = false;
            _camera.SnapCamera();
            Log.DebugMessage(DebugGroup.PixygonMicro, "Level loaded!", this);
        }
        private async Task SetupLevel() {
            if (CurrentLevel != null)
                Destroy(CurrentLevel.gameObject);
            var g = await AddressableLoader.LoadGameObject(CurrentLevelData._levelRef, transform);
            CurrentLevel = g.GetComponent<Level>();
        }
        private async Task SetupPlayer() {
            if(_player == null) 
                _player = await AddressableLoader.LoadGameObject(_playerData._actorRef, transform);
            _player.transform.position = CurrentLevel.PlayerSpawn;
            _camera.Initialize(_player.transform);
            _player.GetComponent<MicroActor>().Initialize(this, _playerData);
        }
        private async Task SetupParallax() {
            if (_parallax == null) {
                var p = await AddressableLoader.LoadGameObject(_parallaxPrefabRef, transform);
                _parallax = p.GetComponent<Parallax.Parallax>();
            }
            _parallax.Initialize(_player.transform, MicroController._instance.Display._camera, CurrentLevelData._parallaxLayerDatas);
        }
        private async Task SetupBgm() {
            GetComponent<AudioSource>().clip = await AddressableLoader.LoadAsset<AudioClip>(CurrentLevelData._bgmRef);
            //GetComponent<AudioSource>().Play();
        }
        private async Task SetupPostProc() {
            MicroController._instance.Display._volume.profile = CurrentLevelData._postProcessingProfileRef != null ?
                await AddressableLoader.LoadAsset<VolumeProfile>(CurrentLevelData._postProcessingProfileRef) : MicroController._instance.Display._defaultVolume;
        }
    }
}