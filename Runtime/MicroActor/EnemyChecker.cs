using System;
using System.Collections.Generic;
using System.Linq;
using Pixygon.DebugTool;
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
        
        public void HandleEnemyCheck(Action onHit) {
            var pos = transform.position;
            var hits = new List<RaycastHit2D>();
            Physics2D.BoxCast(pos+_offset, _size, 0f, Vector2.down, new ContactFilter2D().NoFilter(), hits,  .5f);
            foreach (var hit in hits.Where(hit => hit.collider != null)) {
                var actor = hit.collider.gameObject.GetComponent<MicroActor>();
                if (!actor) continue;
                if (actor == _ignoreActor) continue;
                if (actor.Invincible) continue;
                Log.DebugMessage(DebugGroup.Actor, "Hit something! " + actor.name);
                actor.Damage();
                //TimeEffects.Stop(100);
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