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
        public Sprite _cartridgeBackground;
        public Color _cartridgeColor;
        public NFTLink _nftLink;
        public bool _testingCartridge;

        [Header("Demo / ownership gate")]
        [Tooltip("Levels playable for free without owning the game (the demo 'course'). 0 = no gate (fully open).")]
        public int _freeLevels;
        [Tooltip("Ownership slug checked via PixygonApi.OwnsGame for the full game. Empty = no ownership gate.")]
        public string _ownershipSlug;
        [Tooltip("Storefront URL a non-owner is sent to when they hit the demo limit.")]
        public string _storefrontUrl;
    }
}