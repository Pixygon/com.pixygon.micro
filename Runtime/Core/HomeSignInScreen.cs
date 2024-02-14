using Pixygon.Passport;
using UnityEngine;

namespace Pixygon.Micro {
    public class HomeSignInScreen : MonoBehaviour {
        [SerializeField] private HomeController _home;
        private void Update() {
            while (PixygonApi.Instance.AccountData == null)
                return;
            _home.Activate(true);
            gameObject.SetActive(false);
        }
    }
}