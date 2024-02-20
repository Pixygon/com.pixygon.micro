using System;
using UnityEngine;
using UnityEditor;

namespace Pixygon.Micro {
    [Serializable]
    public class LevelMission {
        [ContextMenuItem("Get Pickups", "GatherPickups")]
        public Pickup[] _pickups;
        [ContextMenuItem("Get ActorSpawners", "GatherActors")]
        public MicroActorSpawner[] _actors;
        [ContextMenuItem("Get LevelObjects", "GatherLevelObjects")]
        public LevelObject[] _levelObjects;
        public Transform[] _playerSpawns;
    }
}