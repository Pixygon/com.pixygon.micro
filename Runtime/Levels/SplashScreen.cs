using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Pixygon.Micro {
    public class SplashScreen : MonoBehaviour {
        [SerializeField] private Sprite[] _splashScreenSprites;
        [SerializeField] private SpriteRenderer _splashScreen;
        [SerializeField] private int _splashDelay = 100;

        public async Task ShowSplashScreens(Action onFinish = null) {
            foreach (var s in _splashScreenSprites) {
                _splashScreen.sprite = s;
                await Task.Delay(_splashDelay);
            }
            onFinish?.Invoke();
        }
    }
}