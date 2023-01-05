using System;
using Pixygon.PagedContent;
using UnityEngine;
using UnityEngine.UI;

namespace Pixygon.Micro {
    public class FaceplateObject : MonoBehaviour {
        [SerializeField] private GameObject _lockIcon;
        [SerializeField] private FaceplateSetter _faceplate;
        
        public bool CanUse { get; private set; }
        
        public void Initialize(Faceplate faceplate) {
            _faceplate.SetFaceplate(faceplate);
            if (faceplate._nftLink.RequiresNFT) {
                _lockIcon.SetActive(true);
                CanUse = false;
                NFT.NFT.ValidateTemplate(faceplate._nftLink.Template[0], Validate);
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