using UnityEngine;

namespace Pixygon.Micro {
    public class CartridgeObject : MonoBehaviour {
        [SerializeField] private GameObject _lockIcon;
        [SerializeField] private MeshRenderer _sticker;

        public bool CanUse { get; private set; }

        private string _currentId;
        public void Initialize(Cartridge cartridge) {
            _sticker.materials[2].SetTexture("_Albedo", cartridge._cartridgeImage);
            _sticker.materials[1].SetColor("_Color", cartridge._cartridgeColor);
            if (cartridge._nftLink.RequiresNFT) {
                _lockIcon.SetActive(true);
                CanUse = false;
                _currentId = cartridge._nftLink.Template[0].collection + cartridge._nftLink.Template[0].schema + cartridge._nftLink.Template[0].template;
                NFT.NFT.ValidateTemplate(cartridge._nftLink.Template[0], Validate);
            }
            else {
                _lockIcon.SetActive(false); 
                CanUse = true;
            }
        }

        private void Validate(string id) {
            if (_currentId != id) return;
            CanUse = true;
            _lockIcon.SetActive(false);
        }
    }
}