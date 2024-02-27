using System.Threading.Tasks;
using Pixygon.Addressable;
using Pixygon.Core;
using Pixygon.DebugTool;
using Pixygon.Passport;
using Pixygon.Saving;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;

namespace Pixygon.Micro {
    public class LevelLoader : MonoBehaviour {
        [SerializeField] private LevelData[] _level;
        [SerializeField] private MissionData[] _missionDatas;
        [SerializeField] private MicroActorData _playerData;
        [SerializeField] private AssetReference _parallaxPrefabRef;
        [SerializeField] private CameraController _camera;
        [SerializeField] private UI _ui;
        [SerializeField] private bool _useFileSelectScreen;
        [SerializeField] private GameObject _fileSelectScreen;
        [SerializeField] private bool _useMapScreen;
        [SerializeField] private GameObject _mapScreen;
        [SerializeField] private bool _useIntroLevel;
        [SerializeField] private int _introId;

        private bool _isSelectingFile;
        private bool _isLoading;
        private GameObject _player;
        private bool _levelLoaded;
        private int _currentLevelId;
        private bool _loadingLevel;
        private int _playerSpawn;
        private bool _fileSelected;

        public UI Ui => _ui;
        public ScoreManager ScoreManager { get; private set; }
        public CameraController Camera => _camera;
        public Level CurrentLevel { get; private set; }
        public LevelData CurrentLevelData { get; private set; }
        public Parallax.Parallax Parallax { get; private set; }
        public LevelData[] Levels => _level;
        public MissionData[] MissionDatas => _missionDatas;
        public int Difficulty { get; private set; }
        public int LoadedLevel { get; private set; }
        public int SelectedMission { get; private set; }
        
        public LevelLoader(ScoreManager scoreManager) {
            ScoreManager = scoreManager;
        }
        private void Awake() {
            PauseManager.ResetPause();
        }
        private void Start() {
            Initialize();
        }
        private void OnEnable() {
            MicroController._instance.Input._jump += SelectLevel;
        }
        private void OnDisable() {
            MicroController._instance.Input._jump -= SelectLevel;
        }
        public void SetDifficulty(int difficulty) {
            Difficulty = difficulty;
        }
        public void OpenFileSelectScreen() {
            if (_isSelectingFile) return;
            Debug.Log("Open file select");
            _isSelectingFile = true;
            _fileSelectScreen.SetActive(true);
        }
        public void CloseFileSelectScreen() {
            Debug.Log("Close file select");
            _isSelectingFile = false;
            _fileSelected = true;
            _fileSelectScreen.SetActive(false);
        }
        public void StartGame(bool playIntro = false) {
            if (MicroController._instance.HomeMenuOpen) return;
            if (_levelLoaded || _loadingLevel) return;
            CloseFileSelectScreen();
            if (_useMapScreen) {
                _mapScreen.SetActive(true);
            } else {
                Log.DebugMessage(DebugGroup.PixygonMicro, "Selected level", this);
                Log.DebugMessage(DebugGroup.PixygonMicro, "Level was not loaded...", this);
                _playerSpawn = 0;
                if(_useIntroLevel && playIntro)
                    StartLevel(_introId);
                else
                    StartLevel(_currentLevelId);
            }
        }
        public void ReturnToMap() {
            Debug.Log("Returning to map!");
            if (_useMapScreen)
                _mapScreen.SetActive(true);
            PauseManager.SetPause(false);
            GetComponent<AudioSource>().Stop();
            Destroy(CurrentLevel.gameObject);
            Destroy(_player.gameObject);
            Destroy(Parallax.gameObject);
            _levelLoaded = false;
            _loadingLevel = false;
            CurrentLevelData = null;
        }
        private void SelectLevel(bool started) {
            if (!started) return;
            if(_useFileSelectScreen && !_fileSelected)
                OpenFileSelectScreen();
            else
                StartGame();
        }
        public void StartLevel(int i) {
            _loadingLevel = true;
            LoadedLevel = i;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Select Level: " + i, this);
            Ui.TriggerMenuScreen(false);
            Ui.PregameScreen.SetActive(true);
            LoadLevel(_level[i]);
        }
        public void SwitchLevel(int level, int playerSpawn = 0, int selectedMission = 0) {
            if (!_levelLoaded) return;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Switch level!", this);
            CurrentLevel.Unload();
            _levelLoaded = false;
            _currentLevelId = level;
            _playerSpawn = playerSpawn;
            SelectedMission = selectedMission;
            StartLevel(_currentLevelId);
        }
        public void EndLevel() {
            if (!_levelLoaded) return;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Ending level!", this);
            _levelLoaded = false;
            _playerSpawn = 0;
            if(_useMapScreen) {
                ReturnToMap();
                return;
            }
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
        private void Initialize() {
            Log.DebugMessage(DebugGroup.PixygonMicro, "Game started", this);
            //Ui.Initialize();
            Ui.TriggerMenuScreen(true);
            ScoreManager = gameObject.AddComponent<ScoreManager>();
            ScoreManager.Initialize(this);
        }
        private async void LoadLevel(LevelData level) {
            if (_levelLoaded) return;
            _levelLoaded = false;
            CurrentLevelData = level;
            Ui.SetLoadScreen(true);
            await SetupBgm();
            await SetupLevel();
            await SetupPlayer();
            await SetupParallax();
            await SetupPostProc();
            Ui.SetLoadScreen(false);
            _levelLoaded = true;
            _loadingLevel = false;
            _camera.SnapCamera();
            Log.DebugMessage(DebugGroup.PixygonMicro, "Level loaded!", this);
        }
        private async Task SetupBgm() {
            GetComponent<AudioSource>().clip = await AddressableLoader.LoadAsset<AudioClip>(CurrentLevelData._bgmRef, f => Ui.LoadScreen.SetLoadPercentage(f*.2f));
            Log.DebugMessage(DebugGroup.PixygonMicro, "Setup BGM", this);
        }
        private async Task SetupLevel() {
            if (CurrentLevel != null)
                Destroy(CurrentLevel.gameObject);
            var g = await AddressableLoader.LoadGameObject(CurrentLevelData._levelRef, transform, true, f => Ui.LoadScreen.SetLoadPercentage(f*.2f+.2f));
            CurrentLevel = g.GetComponent<Level>();
            Log.DebugMessage(DebugGroup.PixygonMicro, "Setup Level", this);
        }
        private async Task SetupPlayer() {
            if (CurrentLevelData._playerOverride != null) {
                if(_player != null)
                    Destroy(_player.gameObject);
                _player = await AddressableLoader.LoadGameObject(CurrentLevelData._playerOverride._actorRef, transform, true, f => Ui.LoadScreen.SetLoadPercentage(f*.2f+.4f));
            } else {
                if(_player == null) 
                    _player = await AddressableLoader.LoadGameObject(_playerData._actorRef, transform, true, f => Ui.LoadScreen.SetLoadPercentage(f*.2f+.4f));
            }
            _player.transform.position = CurrentLevel.PlayerSpawns[_playerSpawn].position;
            _camera.Initialize(_player.transform);
            _player.GetComponent<MicroActor>().Initialize(this, _playerData);
            Log.DebugMessage(DebugGroup.PixygonMicro, "Setup Player", this);
        }
        private async Task SetupParallax() {
            if (!CurrentLevelData._useParallax) return;
            if (Parallax == null) {
                var p = await AddressableLoader.LoadGameObject(_parallaxPrefabRef, transform, true, f => Ui.LoadScreen.SetLoadPercentage(f*.2f+.6f));
                Parallax = p.GetComponent<Parallax.Parallax>();
            }
            Parallax.Initialize(_player.transform, MicroController._instance.Display._camera, CurrentLevelData._parallaxLayerDatas);
            Log.DebugMessage(DebugGroup.PixygonMicro, "Setup Parallax", this);
        }
        private async Task SetupPostProc() {
            MicroController._instance.Display._volume.profile = CurrentLevelData._postProcessingProfileRef != null ?
                await AddressableLoader.LoadAsset<VolumeProfile>(CurrentLevelData._postProcessingProfileRef, f => Ui.LoadScreen.SetLoadPercentage(f*.2f+.8f)) : MicroController._instance.Display._defaultVolume;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Setup PostProc", this);
        }
    }
}