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
            var plate = MicroController._instance.CurrentlyLoadedFaceplate;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Set faceplate to " + plate._title, this);
            _bodyMat.color = plate._color;
            _detailsMat.color = plate._detailColor;
            _buttonsMat.color = plate._buttonColor;
            _faceMat.EnableKeyword("_Albedo");
            _faceMat.SetTexture("_Albedo", plate._tex != null ? plate._tex : null);
            if (plate._useFaceplateColor)
                _faceMat.color = plate._faceplate;
        }

        public void SetZoom(float f) {
            MicroController._instance.SetZoom(f);
        }
    }
}