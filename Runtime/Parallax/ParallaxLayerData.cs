using UnityEngine;

namespace Pixygon.Micro {
    [CreateAssetMenu(menuName = "Pixygon/Micro/New Parallax Layer")]
    public class ParallaxLayerData : ScriptableObject {
        public Sprite _sprite;
        public float _zDistance;
        public Vector2 _tiling;
        public Vector2 _offset;
        public int _sortOrder;
        public bool _lockYAxis;
        public bool _lockXAxis;
    }
}