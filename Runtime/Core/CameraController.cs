using UnityEngine;

namespace Pixygon.Micro {
    public class CameraController : MonoBehaviour {
        [SerializeField] private bool _followPlayer;
        [SerializeField] private Transform _player;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _lag;

        public void Initialize(Transform player) {
            _player = player;
        }

        private void Update() {
            if (_followPlayer) {
                if (_player == null) return;
                MicroController._instance.Display._camera.transform.position = Vector3.Lerp(
                    MicroController._instance.Display._camera.transform.position,
                    (_player.position + _offset) + (Vector3.back * 10f), _lag);
            }
        }
    }
}