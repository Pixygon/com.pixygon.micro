using System;
using System.Linq;
using Pixygon.NFT;
using UnityEngine;

namespace Pixygon.Micro {
    [CreateAssetMenu(menuName = "PixygonMicro/New Faceplate")]
    [Serializable]
    public class Faceplate : ScriptableObject {
        [Header("NFT Info")]
        public string _title;
        public string _collabPartner;
        public string _collabWallet;
        public int _maxSupply;
        public int _price;
        public bool _getImagesFromURL;
        public string _collabIconURL;
        //public Texture2D _collabIcon;
        public Rarity _rarity;
        public NFTLink _nftLink;
        
        [Header("Faceplate")]
        public Color _color;
        public Color _detailColor;
        public Color _buttonColor;
        //public Texture2D _tex;
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

    [Serializable]
    public class FaceplateData {
        public string _title;
        public string _collabPartner;
        public string _collabWallet;
        public int _maxSupply;
        public int _price;
        public bool _getImagesFromURL;
        public string _collabIconURL;
        //public Texture2D _collabIcon;
        public Rarity _rarity;
        public NFTLink _nftLink;
        public Color _color;
        public Color _detailColor;
        public Color _buttonColor;
        //public Texture2D _tex;
        public string _textureURL;
        public bool _useFaceplateColor;
        public Color _faceplate;

        public FaceplateData(Faceplate f) {
            if (f == null)
                Debug.Log("f is null??");
            _title = f._title;
            _collabPartner = f._collabPartner;
            _collabWallet = f._collabWallet;
            _maxSupply = f._maxSupply;
            _price = f._price;
            _getImagesFromURL = f._getImagesFromURL;
            _collabIconURL = f._collabIconURL;
            _rarity = f._rarity;
            _nftLink = f._nftLink;
            _color = f._color;
            _detailColor = f._detailColor;
            _buttonColor = f._buttonColor;
            _textureURL = f._textureURL;
            _useFaceplateColor = f._useFaceplateColor;
            _faceplate = f._faceplate;
        }

        public static string ConvertToJson(Faceplate[] faceplates) {
            var fdl = new FaceplateDataList(faceplates.Select(faceplate => new FaceplateData(faceplate)).ToArray());
            return JsonUtility.ToJson(fdl);
        }
    }

    [Serializable]
    public class FaceplateDataList {

        public FaceplateData[] _data;
        public FaceplateDataList(FaceplateData[] data) {
            _data = data;
        }
    }
}