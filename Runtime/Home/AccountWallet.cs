using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pixygon.Micro {
    public class AccountWallet : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _waxWalletText;
        [SerializeField] private TextMeshProUGUI _ethWalletText;
        [SerializeField] private TextMeshProUGUI _tezWalletText;
        
        [SerializeField] private GameObject _eventWallet;
        [SerializeField] private GameObject _waxCheck;
        [SerializeField] private GameObject _ethCheck;
        [SerializeField] private GameObject _tezCheck;
        [SerializeField] private GameObject _walletLoadingScreen;
        [SerializeField] private GameObject _eventWaxWallets;
        [SerializeField] private GameObject _eventFetching;
        [SerializeField] private GameObject _waxWallets;

        public void OpenWaxWallets() {
            _waxWallets.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_eventWaxWallets);
        }

        public void CloseWaxWallets() {
            _waxWallets.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_eventWallet);
        }
        public void GetWaxWallet() {
            MicroController._instance.GetWaxWallet();
            _walletLoadingScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_eventFetching);
        }
        public void GetAnchorWallet() {
            MicroController._instance.GetAnchorWallet();
            _walletLoadingScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_eventFetching);
        }
        public void GetEthWallet() {
            MicroController._instance.GetEthWallet();
            _walletLoadingScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_eventFetching);
        }
        public void GetTezWallet() {
            MicroController._instance.GetTezWallet();
            _walletLoadingScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_eventFetching);
        }
        public void WalletReceived() {
            _walletLoadingScreen.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_eventWallet);
            CloseWaxWallets();
            RefreshWallets();
        }
        public void OpenWalletScreen() {
            gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_eventWallet);
            RefreshWallets();
        }

        public void RefreshWallets() {
            _waxCheck.SetActive(!string.IsNullOrWhiteSpace(Saving.SaveManager.SettingsSave._user.waxWallet));
            _ethCheck.SetActive(!string.IsNullOrWhiteSpace(Saving.SaveManager.SettingsSave._user.ethWallet));
            _tezCheck.SetActive(!string.IsNullOrWhiteSpace(Saving.SaveManager.SettingsSave._user.tezWallet));
            _waxWalletText.text = Saving.SaveManager.SettingsSave._user.waxWallet;
            _ethWalletText.text = Saving.SaveManager.SettingsSave._user.ethWallet;
            _tezWalletText.text = Saving.SaveManager.SettingsSave._user.tezWallet;
        }
    }
}