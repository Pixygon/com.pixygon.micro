using UnityEngine;

namespace Pixygon.Micro {
    public class IFrameManager : MonoBehaviour {
        private float _iFrameLength;
        private float _iFrames;
        private float _iFrameEffectCounter;
        private bool _iFrameRed;
        private SpriteRenderer _sprite;
        private MicroActor _actor;

        public void Initialize(MicroActor actor, SpriteRenderer sprite) {
            _sprite = sprite;
            _actor = actor;
            _iFrameLength = actor.Data._iFrameLength;
        }
        public void SetIFrames() {
            _actor.Invincible = true;
            _iFrames = _iFrameLength;
        }
        public void HandleIFrames() {
            if (!_actor.Invincible) return;
            if (_iFrames > 0f) {
                _iFrames -= Time.deltaTime;
                if (_iFrameEffectCounter > 0f)
                    _iFrameEffectCounter -= Time.deltaTime;
                else {
                    _iFrameRed = !_iFrameRed;
                    _iFrameEffectCounter = .1f;
                    _sprite.color = _iFrameRed ? Color.red : Color.white;
                }
            }
            else
                StopIFrames();
        }
        public void StopIFrames() {
            _iFrameEffectCounter = 0f;
            _actor.Invincible = false;
            _iFrameRed = false;
            _sprite.color = Color.white;
        }
    }
}