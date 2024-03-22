using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Pixygon.Micro {
    public class SplashScreen : MonoBehaviour {
        [SerializeField] private Sprite[] _splashScreenSprites;
        [SerializeField] private SpriteRenderer _splashScreen;
        [SerializeField] private int _splashDelay = 100;

        public async Task ShowSplashScreens(Action onFinish = null) {
            _splashScreen.gameObject.SetActive(true);
            foreach (var s in _splashScreenSprites) {
                _splashScreen.sprite = s;
                await Task.Delay(_splashDelay);
            }
            _splashScreen.gameObject.SetActive(false);
            onFinish?.Invoke();
        }
    }
}