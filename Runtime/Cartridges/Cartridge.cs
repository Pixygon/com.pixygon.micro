using Pixygon.NFT;
using UnityEngine;

namespace Pixygon.Micro {
    [CreateAssetMenu(menuName = "PixygonMicro/New Cartridge")]
    public class Cartridge : ScriptableObject {
        public string _id;
        public string _title;
        public string _description;
        public string _version;
        public GameObject _gamePrefab;
        public Texture2D _cartridgeImage;
        public Sprite _cartridgeBanner;
        public Color _cartridgeColor;
        public NFTLink _nftLink;
        public bool _testingCartridge;
    }
}