using UnityEngine;

namespace Pixygon.Micro {
    public class CartridgeController : MonoBehaviour {
        [SerializeField] private Intro _introPrefab;

        private GameObject _game;
        private Intro _intro;

        public void Initilize() {
            if (MicroController._instance.SkipIntro)
                LoadCartridge(MicroController._instance.Cartridges[0]);
            else {
                _intro = Instantiate(_introPrefab, transform);
                _intro.StartIntro(EndIntro);
            }
        }

        private void EndIntro() {
            _intro.gameObject.SetActive(false);
            LoadCartridge(MicroController._instance.Cartridges[0]);
        }

        public void LoadCartridge(Cartridge c) {
            _game = Instantiate(c._gamePrefab, transform);
        }

        public void UnloadCartridge() {
            Destroy(_game);
        }
    }
}