using UnityEngine;
using Pixygon.Versioning;
using TMPro;

namespace Pixygon.Micro {
    public class SystemSettingsScreen : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _versionText;
        [SerializeField] private ChangelogMenu _changelog;
        public void OpenScreen(bool open) {
            gameObject.SetActive(open);
            if (!open) return;
            _versionText.text = MicroController._instance.Version;
            _changelog.Initialize(MicroController._instance.Versions);
        }
    }
}