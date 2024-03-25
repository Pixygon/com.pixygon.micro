using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Pixygon.Micro {
    public class SplashScreen : MonoBehaviour {
        [SerializeField] private Sprite[] _splashScreenSprites;
        [SerializeField] private SpriteRenderer _splashScreen;
        [SerializeField] private float _splashDelay = 4000f;

        private float _timer;
        
        public async Task ShowSplashScreens(Action onFinish = null) {
            _splashScreen.gameObject.SetActive(true);
            foreach (var s in _splashScreenSprites) {
                _splashScreen.sprite = s;
                _timer = _splashDelay;
                while (_timer >= 0f) {
                    _timer -= Time.deltaTime;
                    if (MicroController._instance.Input.JumpAction.WasPerformedThisFrame())
                        _timer = .5f;
                    await Task.Yield();
                }
            }
            _splashScreen.gameObject.SetActive(false);
            onFinish?.Invoke();
        }
    }
}