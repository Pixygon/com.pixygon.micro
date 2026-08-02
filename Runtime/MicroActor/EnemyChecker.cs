using System;
using System.Collections.Generic;
using Pixygon.Effects;
using UnityEngine;

namespace Pixygon.Micro
{
    public class EnemyChecker : MonoBehaviour
    {
        [SerializeField] private EffectData _attackFx;
        [SerializeField] private MicroActor _ignoreActor;

        [SerializeField] private Vector3 _offset = Vector3.down * .5f;
        [SerializeField] private Vector3 _size = Vector2.one * 1.2f;

        // Cached so the airborne stomp check (runs every frame while falling/jumping) allocates nothing.
        private readonly List<RaycastHit2D> _hits = new();
        private ContactFilter2D _filter;
        private bool _filterReady;

        public void HandleEnemyCheck(Action onHit) {
            if (!_filterReady) { _filter = new ContactFilter2D().NoFilter(); _filterReady = true; }
            _hits.Clear();
            Physics2D.BoxCast(transform.position + _offset, _size, 0f, Vector2.down, _filter, _hits, .5f);
            for (var i = 0; i < _hits.Count; i++) {
                var col = _hits[i].collider;
                if (col == null) continue;
                if (!col.TryGetComponent<MicroActor>(out var actor)) continue;
                if (actor == _ignoreActor) continue;
                if (actor.Invincible) continue;
                actor.Damage();
                CameraController.Shake(.01f, .2f);
                InputController.Rumble(.1f, .2f);
                EffectsManager.SpawnEffect(_attackFx.GetFullID, transform.position);
                onHit.Invoke();
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position+_offset, _size);
        }
    }
}
