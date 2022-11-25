using UnityEngine;

namespace Pixygon.Micro.Parallax {
    public class ParallaxLayer : MonoBehaviour {
        [SerializeField] private Vector2 _offset;
        [SerializeField] private SpriteRenderer _sprite;
        private Camera _camera;
        private Transform _player;
        private Vector2 _startPos;
        private float _startZ;
        private float _length;
        private bool _lockX;
        private bool _lockY;
        private Vector2 Travel => (Vector2)_camera.transform.localPosition - _startPos;
        private float ClippingPlane => (_camera.transform.localPosition.z +
                                        (DistanceFromSubject > 0 ? _camera.farClipPlane : _camera.nearClipPlane));
        private float ParallaxFactor => Mathf.Abs(DistanceFromSubject) / ClippingPlane;
        private float DistanceFromSubject => transform.localPosition.z - _player.position.z;
        public void Initialize(Transform player, Camera camera, ParallaxLayerData data) {
            _camera = camera;
            _player = player;
            var transform1 = transform;
            var localPosition = transform1.localPosition;
            _startPos = localPosition;
            localPosition = new Vector3(_startPos.x, _startPos.y, data._zDistance);
            transform1.localPosition = localPosition;
            _startZ = localPosition.z;
            _sprite.sprite = data._sprite;
            _sprite.drawMode = SpriteDrawMode.Tiled;
            _sprite.size = data._tiling;
            _sprite.color = data._color;
            _offset = data._offset;
            _sprite.sortingOrder = data._sortOrder;
            _lockX = data._lockXAxis;
            _lockY = data._lockYAxis;
            if (data._isAnimated) {
                gameObject.AddComponent<Animator>().runtimeAnimatorController = data._animator;
            }
            _length = _sprite.sprite.bounds.size.x;
        }
        public void UpdateParallax() {
            //var temp = _camera.transform.position.x * (1 - ParallaxFactor);
            var newPos = (_startPos + _offset) + Travel * ParallaxFactor;
            if (_lockX)
                newPos = new Vector2(_startPos.x+_offset.x, newPos.y);
            if(_lockY)
                newPos = new Vector2(newPos.x, _startPos.y+_offset.y);
            
            transform.localPosition = new Vector3(newPos.x, newPos.y, _startZ);
            //if (temp > _startPos.x + (_length / 2f))
            //    _startPos = new Vector2(_startPos.x + _length, _startPos.y);
            //else if (temp < _startPos.x - (_length / 2f))
            //    _startPos = new Vector2(_startPos.x - _length, _startPos.y);
            //HandleScroll();
        }

        private void HandleScroll() {
            //This fucks up hard!
            _length = 240;
        }
    }
}