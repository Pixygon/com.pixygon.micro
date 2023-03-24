using Pixygon.NFT;
using UnityEngine;

namespace Pixygon.Micro {
    [CreateAssetMenu(menuName = "PixygonMicro/New Faceplate")]
    public class Faceplate : ScriptableObject {
        [Header("NFT Info")]
        public string _title;
        public string _collabPartner;
        public string _collabWallet;
        public int _maxSupply;
        public int _price;
        public bool _getImagesFromURL;
        public string _collabIconURL;
        public Texture2D _collabIcon;
        public Rarity _rarity;
        public NFTLink _nftLink;
        
        [Header("Faceplate")]
        public Color _color;
        public Color _detailColor;
        public Color _buttonColor;
        public Texture2D _tex;
        public string _textureURL;
        public bool _useFaceplateColor;
        public Color _faceplate;
        
    }

    public enum Rarity {
        Infinite,
        Promo,
        Common,
        Scarce,
        Rare,
        Epic,
        Legendary,
        Mythical
    }
}