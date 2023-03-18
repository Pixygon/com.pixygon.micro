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
        
        public void HandleEnemyCheck(Action onHit) {
            var pos = transform.position;
            Debug.DrawRay(pos, Vector3.down * .4f);
            var hits = new List<RaycastHit2D>();
            Physics2D.BoxCast(pos, Vector2.one * .8f, 0f, Vector2.down, new ContactFilter2D().NoFilter(), hits,  .5f);
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
    }
}