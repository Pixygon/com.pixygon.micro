﻿using TMPro;
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
        [SerializeField] private Animator _pointsAnimator;
        [SerializeField] private Animator _coinsAnimator;
        
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

        public void SetPoints(int points) {
            if(_animatePoints) _pointsAnimator.SetTrigger("Tick");
            _pointsText.text = points.ToString();
        }
        public void TriggerMenuScreen(bool activate) {
            _menuScreen.SetActive(activate);
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