using System.Collections;
using UnityEngine;

namespace Pixygon.Micro {
    public class CameraController : MonoBehaviour {
        [SerializeField] private bool _followPlayer;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _lag;

        private Transform _player;
        private Camera _cam;      // cached in Initialize to avoid the MicroController._instance.Display._camera
        private Transform _camT;  // property chain every LateUpdate
        private Vector3 _shakeOffset;
        private static CameraController _active; // the follow cam currently driving the screen
        private static readonly AnimationCurve ShakeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f); // built once

        public Vector3 Offset {
            get => _offset;
            set => _offset = value;
        }

        public void Initialize(Transform player) {
            _player = player;
            _active = this;
            _cam = MicroController._instance.Display._camera;
            _camT = _cam.transform;
        }

        // LateUpdate so the camera follows the player's post-move position (no one-frame trailing jitter).
        private void LateUpdate() {
            if (!_followPlayer) return;
            if (_player == null || _cam == null) return;
            var target = _player.position + _offset;
            var pos = Vector3.Lerp(_camT.position, target, _lag);
            // Leash: the smooth lerp alone can't keep up with a fast fall, so the player would slide off-screen.
            // Clamp the camera to stay within ~70% of the view of the player, guaranteeing they stay framed
            // while still lagging smoothly during normal movement.
            if (_cam.orthographic) {
                var halfH = _cam.orthographicSize * 0.7f;
                var halfW = _cam.orthographicSize * _cam.aspect * 0.7f;
                pos.x = Mathf.Clamp(pos.x, target.x - halfW, target.x + halfW);
                pos.y = Mathf.Clamp(pos.y, target.y - halfH, target.y + halfH);
            }
            pos.z = target.z;
            _camT.position = pos + _shakeOffset; // shake is layered on top of the follow so it actually shows
        }
        public void SnapCamera() {
            if (_camT != null) _camT.position = _player.position + _offset;
        }

        // Shake as a decaying offset that LateUpdate adds to the follow position (the old version set the
        // camera position directly, which the follow overwrote every frame — so it never showed). Runs on
        // unscaled time so it still plays during a hit-stop freeze.
        public static void Shake(float duration, float intensity = 1f) {
            if (_active != null) _active.StartCoroutine(_active.DoShake(duration, intensity));
        }
        private IEnumerator DoShake(float duration, float intensity) {
            var t = 0f;
            while (t < duration) {
                t += Time.unscaledDeltaTime;
                _shakeOffset = (Vector3)(Random.insideUnitCircle * (intensity * ShakeCurve.Evaluate(t / duration)));
                yield return null;
            }
            _shakeOffset = Vector3.zero;
        }
    }
}
