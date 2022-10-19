using UnityEngine;

namespace Pixygon.Micro {
    [CreateAssetMenu(menuName = "PixygonMicro/New Faceplate")]
    public class Faceplate : ScriptableObject {
        public string _title;
        public Color _color;
        public Sprite _sprite;
    }
}