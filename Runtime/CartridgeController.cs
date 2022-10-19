using UnityEngine;

namespace Pixygon.Micro {
    public class CartridgeController : MonoBehaviour {
        [SerializeField] private Cartridge _cartridge;
        [SerializeField] private bool _skipIntro;
        [SerializeField] private Intro _introPrefab;

        private GameObject _game;
        private Intro _intro;

        public void Initilize() {
            if (_skipIntro)
                LoadCartridge(_cartridge);
            else {
                _intro = Instantiate(_introPrefab, transform);
                _intro.StartIntro(EndIntro);
            }
        }

        private void EndIntro() {
            _intro.gameObject.SetActive(false);
            LoadCartridge(_cartridge);
        }

        public void LoadCartridge(Cartridge c) {
            _game = Instantiate(c._gamePrefab, transform);
        }

        public void UnloadCartridge() {
            Destroy(_game);
        }
    }
}