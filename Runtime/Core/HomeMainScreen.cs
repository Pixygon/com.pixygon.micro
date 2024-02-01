using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixygon.Passport;

namespace Pixygon.Micro {
    public class HomeMainScreen : MonoBehaviour
    {
        [SerializeField] private PassportBadge _passportBadge;
        [SerializeField] private RectTransform _gameSlotsRect;
        [SerializeField] private GameObject _gameSlotPrefab;
        [SerializeField] private Image _bg;
        [SerializeField] private HomeController _home;
        [SerializeField] private GameBgSet _gameBgAnimated;

        private List<GameSlot> _slots;
        private int _currentlyActiveSlot;
        public PassportBadge PassportBadge => _passportBadge;
        public void Activate() {
            SetUsernameText();
            Clear();
            var i = 0;
            foreach (var c in MicroController._instance.Cartridges) {
                var slot = Instantiate(_gameSlotPrefab, _gameSlotsRect).GetComponent<GameSlot>();
                slot.Set(c, this, i);
                _slots.Add(slot);
                i++;
            }
            _gameSlotsRect.sizeDelta = new Vector2(330 * _slots.Count, _gameSlotsRect.sizeDelta.y);
            _currentlyActiveSlot = 0;
            _slots[_currentlyActiveSlot].Select();
        }

        public void Clear() {
            if(_slots != null) {
                foreach (var g in _slots) {
                    Destroy(g.gameObject);
                }
            }
            _slots = new List<GameSlot>();
        }

        public void SetActiveSlot(int i) {
            _slots[_currentlyActiveSlot].Deselect();
            _currentlyActiveSlot = i;
            _slots[_currentlyActiveSlot].Select();
            _gameBgAnimated.SetNewBg(_slots[_currentlyActiveSlot].Cartridge._cartridgeBackground);
            if (!_slots[_currentlyActiveSlot].CanUse) return;
            if(_currentlyActiveSlot > MicroController._instance.Cartridges.Length)
                _currentlyActiveSlot = 0;
            PlayerPrefs.SetInt("Cartridge", _currentlyActiveSlot);
            PlayerPrefs.Save();
            _home.SetCurrentCartridge();
        }
        public void SetUsernameText() {
            _passportBadge.Set();
        }

        public void SetBg() {
            _bg.sprite = _slots[_currentlyActiveSlot].Cartridge._cartridgeBackground;
        }
    }
}