using UnityEngine;

namespace Pixygon.Micro
{
    public class ConsoleController : MonoBehaviour {
        [SerializeField] private Material _bodyMat;
        [SerializeField] private Material _faceMat;
        public void Initialize() {
            UpdateFaceplate();
        }
        public void UpdateFaceplate() {
            _bodyMat.color = MicroController._instance.CurrentlyLoadedFaceplate._color;
            _faceMat.mainTexture = MicroController._instance.CurrentlyLoadedFaceplate._tex != null ? MicroController._instance.CurrentlyLoadedFaceplate._tex : null;
        }

        public void SetZoom(float f) {
            MicroController._instance.SetZoom(f);
        }
    }
}