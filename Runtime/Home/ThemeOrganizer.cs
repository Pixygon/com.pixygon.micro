using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixygon.Micro
{
    public class ThemeOrganizer : MonoBehaviour {

        public Themes _currentTheme;

        [SerializeField] private Sprite _default;
        [SerializeField] private Sprite _defaultBlue;
        [SerializeField] private Sprite _defaultGreen;
        [SerializeField] private Sprite _defaultRed;
        [SerializeField] private Sprite _defaultYellow;

        private void Awake() {
            GetComponent<SpriteRenderer>().sprite = _currentTheme switch {
                Themes.Default => _default,
                Themes.DefaultBlue => _defaultBlue,
                Themes.DefaultGreen => _defaultGreen,
                Themes.DefaultRed => _defaultRed,
                Themes.DefaultYellow => _defaultYellow,
                _ => GetComponent<SpriteRenderer>().sprite
            };
        }
    }

    public enum Themes {
        Default,
        DefaultBlue,
        DefaultGreen,
        DefaultRed,
        DefaultYellow,
    }
}