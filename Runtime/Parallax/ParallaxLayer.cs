﻿using UnityEngine;

namespace Pixygon.Micro {
    public class ParallaxLayer : MonoBehaviour {
        [SerializeField] private Vector2 _offset;
        [SerializeField] private SpriteRenderer _sprite;
        private Camera _camera;
        private Transform _player;
        private Vector2 _startPos;
        private float _startZ;
        public void Initialize(Transform player, ParallaxLayerData data) {
            _camera = MicroController._instance.Display._camera;
            _player = player;
            _startPos = transform.localPosition;
            transform.localPosition = new Vector3(_startPos.x, _startPos.y, data._zDistance);
            _startZ = transform.localPosition.z;
            _sprite.sprite = data._sprite;
            _sprite.drawMode = SpriteDrawMode.Tiled;
            _sprite.size = data._tiling;
            _offset = data._offset;
            _sprite.sortingOrder = data._sortOrder;
        }
        public Vector2 Travel => (Vector2)_camera.transform.localPosition - _startPos;
        private float ClippingPlane => (_camera.transform.localPosition.z +
                                        (DistanceFromSubject > 0 ? _camera.farClipPlane : _camera.nearClipPlane));
        public float ParallaxFactor => Mathf.Abs(DistanceFromSubject) / ClippingPlane;
        public float DistanceFromSubject => transform.localPosition.z - _player.position.z;
        public void UpdateParallax() {
            var newPos = (_startPos + _offset) + Travel * ParallaxFactor;
            transform.localPosition = new Vector3(newPos.x, newPos.y, _startZ);
        }
    }
}