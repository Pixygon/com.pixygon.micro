using System;
using Pixygon.PagedContent;
using UnityEngine;
using UnityEngine.UI;

namespace Pixygon.Micro {
    public class FaceplateObject : PagedContentObject {
        [SerializeField] private GameObject _lockIcon;
        private bool _canUse;
        public override void Initialize(object d, int num, Action<object, int> a) {
            base.Initialize(d, num, a);
            var faceplate = d as Faceplate;
            //_text.text = faceplate._title;
            if (faceplate._nftLink.RequiresNFT) {
                _lockIcon.SetActive(true);
                NFT.NFT.ValidateTemplate(faceplate._nftLink.Template[0], () => { _canUse = true; _lockIcon.SetActive(false); });
            }
            else {
                _canUse = true;
                _lockIcon.SetActive(false);
            }
            if (faceplate._useFaceplateColor) {
                GetComponent<Image>().sprite = Sprite.Create(faceplate._tex, new Rect(0, 0, faceplate._tex.width, faceplate._tex.height), new Vector2(.5f, .5f));
                GetComponent<Image>().color = faceplate._faceplate;
            } else {
                GetComponent<Image>().sprite = Sprite.Create(faceplate._tex, new Rect(0, 0, faceplate._tex.width, faceplate._tex.height), new Vector2(.5f, .5f));
                GetComponent<Image>().color = Color.white;
            }
        }

        public override void Activate() {
            if (!_canUse) return;
            base.Activate();
        }
    }
}