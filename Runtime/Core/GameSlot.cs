using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pixygon.Micro
{
    public class GameSlot : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Image _highlight;
        [SerializeField] private Image _gameIcon;
        [SerializeField] private RectTransform _rect;
        [SerializeField] private Color _highlightColor;
        [SerializeField] private Color _emptyColor;
        [SerializeField] private GameObject _startBtn;
        
        private HomeMainScreen _mainScreen;
        private int _count;
        private string _currentId;
        public Cartridge Cartridge;
        private bool _isActive;
        public bool CanUse { get; private set; }
        public void Set(Cartridge cartridge, HomeMainScreen home, int i) {
            Cartridge = cartridge;
            _mainScreen = home;
            _count = i;
            _gameIcon.sprite = cartridge._cartridgeBanner;
            _startBtn.SetActive(false); 
            if (cartridge._nftLink.RequiresNFT) {
                CanUse = false;
                var verificationId = cartridge._id;
                _currentId = verificationId;
                foreach (var s in cartridge._nftLink.Template) {
                    NFT.NFT.ValidateTemplate(s, Validate, null, verificationId);
                }
            } else {
                CanUse = true;
            }
        }

        public void OnClick() {
            _mainScreen.SetActiveSlot(_count);
        }

        public void OnClickPlay() {
            MicroController._instance.Home.StartGame();
        }

        public void Select() {
            _titleText.text = Cartridge._title;
            _highlight.color = _highlightColor;
            _rect.sizeDelta = new Vector2(330f, 330f);
            _startBtn.SetActive(CanUse);
            _isActive = true;
            GetComponent<Animator>().SetTrigger("Select");
        }
        public void Deselect() {
            _titleText.text = "";
            _highlight.color = Color.clear;
            _rect.sizeDelta = new Vector2(300f, 300f);
            _startBtn.SetActive(false);
            _isActive = false;
            GetComponent<Animator>().SetTrigger("Deselect");
        }

        private void Validate(string id) {
            if (_currentId != id) return;
            Debug.Log("Valid!");
            CanUse = true;
            _startBtn.SetActive(_isActive);
        }
    }
}
