using UnityEngine;

namespace Pixygon.Micro {
    [CreateAssetMenu(menuName = "PixygonMicro/New Cartridge")]
    public class Cartridge : ScriptableObject {
        public string _title;
        public string _description;
        public GameObject _gamePrefab;
        public Texture2D _cartridgeImage;
        public Color _cartridgeColor;
    }
}