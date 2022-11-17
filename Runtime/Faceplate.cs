using UnityEngine;

namespace Pixygon.Micro {
    [CreateAssetMenu(menuName = "PixygonMicro/New Faceplate")]
    public class Faceplate : ScriptableObject {
        public string _title;
        public Color _color;
        public Color _detailColor;
        public Color _buttonColor;
        public Texture2D _tex;
        public bool _useFaceplateColor;
        public Color _faceplate;
    }
}