using Pixygon.DebugTool;
using UnityEngine;

namespace Pixygon.Micro {
    public class CartridgeController : MonoBehaviour {
        [SerializeField] private Intro _introPrefab;

        private GameObject _game;
        private Intro _intro;

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
            var i = PlayerPrefs.GetInt("Cartridge");
            if (MicroController._instance.Cartridges == null || MicroController._instance.Cartridges.Length <= i) {
                Log.DebugMessage(DebugGroup.PixygonMicro, "No cartridges set!", this);
                MicroController._instance.TriggerHomeMenu(true);
                return;
            }
            LoadCartridge(MicroController._instance.Cartridges[i]);
        }

        public void LoadCartridge(Cartridge c) {
            _game = Instantiate(c._gamePrefab, transform);
        }

        public void UnloadCartridge() {
            Destroy(_game);
        }
    }
}