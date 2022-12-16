using UnityEngine;

namespace Pixygon.Micro {
    public class WalletFetcher : MonoBehaviour {
        
#if UNITY_WEBGL
        public void GetAddress() {
            WebGLDispatcher.Login();
        }

        public void GotWallet(string wallet) {
            PlayerPrefs.SetString("WaxWallet", wallet);
            PlayerPrefs.Save();
            MicroController._instance.SetWallet(wallet);
        }

#endif
    }
}