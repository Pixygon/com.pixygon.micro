using UnityEngine;

namespace Pixygon.Micro
{
    public class ConsoleController : MonoBehaviour {
        [SerializeField] private Material _bodyMat;
        [SerializeField] private Material _faceMat;
        [SerializeField] private Faceplate _defaultFaceplate;
        public void Initialize() {
            SetFaceplate(_defaultFaceplate);
        }
        public void SetFaceplate(Faceplate faceplate) {
            _bodyMat.color = faceplate._color;
            _faceMat.mainTexture = faceplate._tex != null ? faceplate._tex : null;
        }

        public void SetZoom(float f) {
            MicroController._instance.SetZoom(f);
        }
    }
}