using TMPro;
using UnityEngine;

namespace Pixygon.Micro {
    public class UI : MonoBehaviour {
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private GameObject _menuScreen;
        [SerializeField] private GameObject _levelStartScreen;
        [SerializeField] private GameObject _levelEndScreen;
        [SerializeField] private GameObject _pregameScreen;
        [SerializeField] private GameObject[] _lifeIcons;
        [SerializeField] private TextMeshPro _coinText;
        [SerializeField] private TextMeshPro _pointsText;
        [SerializeField] private TextMeshPro _levelStartText;

        [SerializeField] private bool _animatePoints;
        [SerializeField] private bool _animateCoins;
        [SerializeField] private bool _animateCollectables;
        [SerializeField] private Animator _pointsAnimator;
        [SerializeField] private Animator _coinsAnimator;
        [SerializeField] private Animator _collectablesAnimator;
        
        [SerializeField] private SpriteRenderer[] _collectableSprites;

        [SerializeField] private SpriteRenderer _powerupRenderer;
        [SerializeField] private Sprite[] _powerups;
        [SerializeField] private TextMeshPro _versionText;
        
        public GameObject PregameScreen => _pregameScreen;
        public GameObject LevelEndScreen => _levelEndScreen;
        public void SetLife(int i) {
            _lifeIcons[0].SetActive(i >= 0);
            _lifeIcons[1].SetActive(i >= 1);
            _lifeIcons[2].SetActive(i >= 2);
        }

        public void SetCoins(int coins) {
            if(_animateCoins) _coinsAnimator.SetTrigger("Tick");
            _coinText.text = coins.ToString();
        }
        public void SetPowerup(int i) {
            if (i == -1) {
                _powerupRenderer.gameObject.SetActive(false);
            }
            else {
                _powerupRenderer.sprite = _powerups[i];
                _powerupRenderer.gameObject.SetActive(true);
            }
        }

        public void SetPoints(int points) {
            if(_animatePoints) _pointsAnimator.SetTrigger("Tick");
            _pointsText.text = points.ToString();
        }

        public void SetCollectable(bool[] collectable) {
            if(_animateCollectables) _collectablesAnimator.SetTrigger("Tick");
            for (var i = 0; i < collectable.Length; i++) {
                _collectableSprites[i].color = collectable[i] ? Color.white : Color.black;
            }
        }
        public void TriggerMenuScreen(bool activate) {
            _menuScreen.SetActive(activate);
            if(MicroController._instance.CurrentlyLoadedCartridge != null)
                _versionText.text = MicroController._instance.CurrentlyLoadedCartridge._version;
        }

        public void TriggerGameOverScreen(bool activate) {
            _gameOverScreen.SetActive(activate);
        }

        public void TriggerStartScreen(bool activate, string text = "") {
            _levelStartText.text = text;
            _levelStartScreen.SetActive(activate);
        }
    }
}