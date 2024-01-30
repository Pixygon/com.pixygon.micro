using UnityEngine;
using UnityEngine.UI;

namespace Pixygon.Micro {
    public class GameBgSet : MonoBehaviour {
        [SerializeField] private HomeMainScreen _home;
        [SerializeField] private Image _image;
        public void SetBg() {
            _home.SetBg();
        }

        public void SetNewBg(Sprite s) {
            _image.sprite = s;
            GetComponent<Animator>().SetTrigger("Switch");
        }
    }
}