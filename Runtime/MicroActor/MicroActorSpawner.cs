using Pixygon.DebugTool;
using Pixygon.Addressable;
using UnityEngine;

namespace Pixygon.Micro {
    public class MicroActorSpawner : MonoBehaviour {
        [SerializeField] private MicroActorData _actorData;

        private GameObject _spawnedActor;
        
        public async void SpawnActor() {
            if(_spawnedActor != null) DespawnActor();
            Log.DebugMessage(DebugGroup.Actor, "Spawning actor", this);
            _spawnedActor = await AddressableLoader.LoadGameObject(_actorData._actorRef, transform);
            _spawnedActor.transform.localPosition = Vector3.zero;
        }

        private void DespawnActor() {
            Destroy(_spawnedActor);
        }
    }
}