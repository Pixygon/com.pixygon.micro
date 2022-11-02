using UnityEngine;

namespace Pixygon.Micro
{
    public class ConsoleController : MonoBehaviour {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Faceplate _defaultFaceplate;
        public void Initialize() {
            SetFaceplate(_defaultFaceplate);
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