using UnityEngine;

namespace Pixygon.Micro {
    public class WalletFetcher : MonoBehaviour {
        
#if UNITY_WEBGL
        public void GetWaxAddress() {
            WebGLDispatcher.Wax_Login();
        }
        public void GetAnchorAddress() {
            WebGLDispatcher.Anchor_Login();
        }
        public void GetEthAddress() {
            WebGLDispatcher.Eth_Login();
        }
        public void GetTezAddress() {
            WebGLDispatcher.Tez_Login();
        }
        public void GotWaxWallet(string wallet) {
            MicroController._instance.SetWaxWallet(wallet);
        }
        public void GotEthWallet(string wallet) {
            Debug.Log("Got Eth-wallet");
            MicroController._instance.SetEthWallet(wallet);
        }
        public void GotTezWallet(string wallet) {
            Debug.Log("Got TEZ-wallet");
            MicroController._instance.SetTezWallet(wallet);
        }
#endif
    }
}