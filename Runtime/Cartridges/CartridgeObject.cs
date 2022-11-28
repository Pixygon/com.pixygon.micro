using System;
using Pixygon.PagedContent;
using UnityEngine;
using UnityEngine.UI;

namespace Pixygon.Micro {
    public class CartridgeObject : PagedContentObject {
        private bool _canUse;

        public override void Initialize(object d, int num, Action<object, int> a) {
            base.Initialize(d, num, a);
            var cartridge = d as Cartridge;
            if (cartridge._nftLink.RequiresNFT)
                NFT.NFT.ValidateTemplate(cartridge._nftLink.Template[0], () => { _canUse = true; });
            else
                _canUse = true;
            GetComponent<Image>().sprite = Sprite.Create(cartridge._cartridgeImage,
                new Rect(0, 0, cartridge._cartridgeImage.width, cartridge._cartridgeImage.height), new Vector2(.5f, .5f));
            GetComponent<Image>().color = Color.white;
        }

        public override void Activate() {
            if (!_canUse) return;
            base.Activate();
        }
    }
}