using UnityEngine;

namespace Pixygon.Micro {
    public class CartridgeObject : MonoBehaviour {
        [SerializeField] private GameObject _lockIcon;
        [SerializeField] private MeshRenderer _sticker;

        public bool CanUse { get; private set; }
        public void Initialize(Cartridge cartridge) {
            _sticker.materials[2].SetTexture("_Albedo", cartridge._cartridgeImage);
            if (cartridge._nftLink.RequiresNFT) {
                _lockIcon.SetActive(true);
                CanUse = false;
                NFT.NFT.ValidateTemplate(cartridge._nftLink.Template[0], Validate);
            }
            else {
                _lockIcon.SetActive(false); 
                CanUse = true;
            }
        }

        private void Validate() {
            CanUse = true;
            _lockIcon.SetActive(false);
        }
    }
}