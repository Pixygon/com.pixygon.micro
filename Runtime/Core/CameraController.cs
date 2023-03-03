using System.Threading.Tasks;
using UnityEngine;

namespace Pixygon.Micro {
    public class CameraController : MonoBehaviour {
        [SerializeField] private bool _followPlayer;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _lag;
        
        private Transform _player;
        
        public Vector3 Offset {
            get { return _offset; }
            set { _offset = value; }
        }

        public void Initialize(Transform player) {
            _player = player;
        }

        private void Update() {
            if (!_followPlayer) return;
            if (_player == null) return;
            MicroController._instance.Display._camera.transform.position = Vector3.Lerp(
                MicroController._instance.Display._camera.transform.position, _player.position + _offset, _lag);
        }
        public void SnapCamera() {
            MicroController._instance.Display._camera.transform.position = _player.position + _offset;
        }
        public static async void Shake(float duration, float intensity = 1f) {
            var curve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            var transform1 = MicroController._instance.Display._camera.transform;
            var startPos = transform1.position;
            var time = 0f;
            while (time < duration) {
                time += Time.deltaTime;
                var shake = startPos + (Vector3)Random.insideUnitCircle * (intensity*curve.Evaluate(time/duration));
                transform1.position = new Vector3(shake.x, shake.y, transform1.position.z);
                await Task.Yield();
            }
            transform1.position = startPos;
        }
    }
}
