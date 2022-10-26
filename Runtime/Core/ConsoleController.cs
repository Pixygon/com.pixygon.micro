using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Pixygon.Micro
{
    public class ConsoleController : MonoBehaviour {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Faceplate _defaultFaceplate;
        [SerializeField] private TextMeshPro _versionText;
        public void Initialize() {
            SetFaceplate(_defaultFaceplate);
            _versionText.text = MicroController._instance.Version;
        }
        public void SetFaceplate(Faceplate faceplate) {
            _sprite.color = faceplate._color;
            _sprite.sprite = faceplate._sprite;
        }

        public void SetZoom(float f) {
            MicroController._instance.SetZoom(f);
        }
    }
}
