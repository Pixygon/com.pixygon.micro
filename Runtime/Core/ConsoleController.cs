using System;
using Pixygon.DebugTool;
using UnityEngine;

namespace Pixygon.Micro
{
    public class ConsoleController : MonoBehaviour {
        [SerializeField] private Material _faceMat;
        [SerializeField] private Material _bodyMat;
        [SerializeField] private Material _detailsMat;
        [SerializeField] private Material _buttonsMat;
        public void Initialize() {
            UpdateFaceplate();
        }

        public void UpdateFaceplate() {
            Log.DebugMessage(DebugGroup.PixygonMicro, "Set faceplate to " + MicroController._instance.CurrentlyLoadedFaceplate._title, this);
            _bodyMat.color = MicroController._instance.CurrentlyLoadedFaceplate._color;
            _detailsMat.color = MicroController._instance.CurrentlyLoadedFaceplate._detailColor;
            _buttonsMat.color = MicroController._instance.CurrentlyLoadedFaceplate._buttonColor;
            _faceMat.EnableKeyword("_Albedo");
            _faceMat.SetTexture("_Albedo", MicroController._instance.CurrentlyLoadedFaceplate._tex != null ? MicroController._instance.CurrentlyLoadedFaceplate._tex : null);
        }

        public void SetZoom(float f) {
            MicroController._instance.SetZoom(f);
        }
    }
}