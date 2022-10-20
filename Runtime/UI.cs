﻿using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Pixygon.Micro {
    public class UI : MonoBehaviour {
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private GameObject _menuScreen;
        [SerializeField] private GameObject[] _lifeIcons;
        [SerializeField] private TextMeshPro _coinText;

        public void SetLife(int i) {
            _lifeIcons[0].SetActive(i >= 0);
            _lifeIcons[1].SetActive(i >= 1);
            _lifeIcons[2].SetActive(i >= 2);
        }
        public void SetCoins(int coins) {
            _coinText.text = $"Coins: {coins}";
        }
        public void TriggerMenuScreen(bool activate) {
            //gameObject.SetActive(activate);
            _menuScreen.SetActive(activate);
        }

        public void TriggerGameOverScreen(bool activate) {
            //gameObject.SetActive(activate);
            _gameOverScreen.SetActive(activate);
        }
    }
}