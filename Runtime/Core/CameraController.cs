using System.Threading.Tasks;
using UnityEngine;

namespace Pixygon.Micro {
    public class CameraController : MonoBehaviour {
        [SerializeField] private bool _followPlayer;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _lag;
        
        private Transform _player;

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
        
        public static async void Shake(float duration, float intensity = 1f) {
            Vector2 startPos = MicroController._instance.Display._camera.transform.position;
            var time = 0f;
            while (time < duration) {
                time += Time.deltaTime;
                var shake = startPos + Random.insideUnitCircle * intensity;
                MicroController._instance.Display._camera.transform.position = new Vector3(shake.x, shake.y, MicroController._instance.Display._camera.transform.position.z);
                await Task.Yield();
            }

            MicroController._instance.Display._camera.transform.position = startPos;
        }
    }
}