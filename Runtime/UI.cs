using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Pixygon.Micro {
    public class UI : MonoBehaviour {
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private GameObject _menuScreen;
        [SerializeField] private Camera _uiCamera;

        public void Initialize() {
            MicroController._instance.Display._camera.GetUniversalAdditionalCameraData().cameraStack.Add(_uiCamera);
        }
        public void TriggerMenuScreen(bool activate) {
            gameObject.SetActive(activate);
            _menuScreen.SetActive(activate);
        }

        public void TriggerGameOverScreen(bool activate) {
            gameObject.SetActive(activate);
            _gameOverScreen.SetActive(activate);
        }
    }
}