using UnityEngine.AddressableAssets;
using Pixygon.ID;
using UnityEngine;

namespace Pixygon.Micro {
    [CreateAssetMenu(menuName = "Pixygon/MicroActor", fileName = "New MicroActorData")]
    public class MicroActorData : IdObject {
        public AssetReference _actorRef;
        public float _speed;
        public int _hp;
    }
}