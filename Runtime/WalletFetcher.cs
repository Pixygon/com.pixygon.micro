using Pixygon.NFT;
using UnityEngine;

namespace Pixygon.Micro {
    public class WalletFetcher : MonoBehaviour {
        public void GetWallet(NFT.Chain chain, int walletProvider) {
            #if UNITY_WEBGL
            switch (chain) {
                case Chain.Wax:
                    if(walletProvider == 0)
                        WebGLDispatcher.Wax_Login();
                    if(walletProvider == 1)
                        WebGLDispatcher.Anchor_Login();
                    break;
                case Chain.EOS:
                    break;
                
                case Chain.Ethereum:
                    WebGLDispatcher.Eth_Login();
                    break;
                case Chain.Tezos:
                    WebGLDispatcher.Tez_Login();
                    break;
                case Chain.Polygon:
                    break;
                case Chain.Solana:
                    break;
                case Chain.Flow:
                    break;
            }
            #endif
        }
        public void GotWaxWallet(string wallet) {
            MicroController._instance.SetWallet(Chain.Wax, wallet);
        }
        public void GotEthWallet(string wallet) {
            MicroController._instance.SetWallet(Chain.Ethereum, wallet);
        }
        public void GotTezWallet(string wallet) {
            Debug.Log("Got TEZ-wallet");
            MicroController._instance.SetWallet(Chain.Tezos, wallet);
        }
    }
}