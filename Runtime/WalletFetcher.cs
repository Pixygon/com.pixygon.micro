using UnityEngine;

namespace Pixygon.Micro {
    public class WalletFetcher : MonoBehaviour {
        
#if UNITY_WEBGL
        public void GetWaxAddress() {
            WebGLDispatcher.Wax_Login();
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
            MicroController._instance.SetEthWallet(wallet);
        }

#endif
    }
}
