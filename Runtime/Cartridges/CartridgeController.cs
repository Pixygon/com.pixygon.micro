using Pixygon.DebugTool;
using Pixygon.Saving;
using UnityEngine;

namespace Pixygon.Micro {
    public class CartridgeController : MonoBehaviour {
        [SerializeField] private Intro _introPrefab;

        private GameObject _game;
        private Intro _intro;

        public LevelLoader LevelLoader { get; private set; }
        public void Initilize() {
            #if UNITY_EDITOR
            if (MicroController._instance.SkipIntro)
                StartGame();
            else
            #endif
                StartIntro();
        }

        private void StartIntro() {
            _intro = Instantiate(_introPrefab, transform);
            _intro.StartIntro(EndIntro);
        }

        private void EndIntro() {
            _intro.gameObject.SetActive(false);
            StartGame();
        }

        public void StartGame() {
            if(_game != null)
                Destroy(_game);
            var i = PlayerPrefs.GetInt("Cartridge", -1);
            if (MicroController._instance.Cartridges == null || MicroController._instance.Cartridges.Length <= i || SaveManager.SettingsSave == null) {
                NoCartridge();
                return;
            }
            if (SaveManager.SettingsSave._waxWallet == string.Empty) {
                NoCartridge();
                return;
            }
            if(i == -1) {
                NoCartridge();
                return;
            }
            LoadCartridge(MicroController._instance.Cartridges[i]);
        }

        private void NoCartridge() {
            PlayerPrefs.SetInt("Cartridge", -1);
            Log.DebugMessage(DebugGroup.PixygonMicro, "No cartridges set!", this);
            MicroController._instance.TriggerHomeMenu(true);
        }

        public void LoadCartridge(Cartridge c) {
            _game = Instantiate(c._gamePrefab, transform);
            LevelLoader = _game.GetComponent<LevelLoader>();
        }

        public void UnloadCartridge() {
            Destroy(_game);
        }
    }
}