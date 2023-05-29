using Pixygon.Micro;
using Pixygon.NFT;
using UnityEngine;

namespace Pixygon.Micro {
    [CreateAssetMenu(menuName = "PixygonMicro/New SkinCard")]
    public class SkinCard : ScriptableObject {
        public string _title;
        public string _collabPartner;
        public int _maxSupply;
        public int _price;
        public string _cardNumber;
        public Texture2D _collabIcon;
        public Texture2D _gameIcon;
        public Texture2D _gameBg;
        public Sprite _skinSprite;
        public Rarity _rarity;
        public NFTLink _nftLink;
    }
}