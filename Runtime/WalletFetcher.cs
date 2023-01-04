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

        public void GotWallet(string wallet) {
            PlayerPrefs.SetString("WaxWallet", wallet);
            PlayerPrefs.Save();
            MicroController._instance.SetWallet(wallet);
        }

#endif
    }
}
