using Pixygon.NFT;
using UnityEngine;

namespace Pixygon.Micro {
    [CreateAssetMenu(menuName = "PixygonMicro/New Faceplate")]
    public class Faceplate : ScriptableObject {
        public string _title;
        public string _collabPartner;
        public Texture2D _collabIcon;
        public Rarity _rarity;
        public Color _color;
        public Color _detailColor;
        public Color _buttonColor;
        public Texture2D _tex;
        public bool _useFaceplateColor;
        public Color _faceplate;
        public NFTLink _nftLink;
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