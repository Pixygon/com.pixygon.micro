using System;
using UnityEngine;

namespace Pixygon.Micro {
    [Serializable]
    public class LevelMission {
        public GameObject _missionObject;
        public Pickup[] _pickups;
        public MicroActorSpawner[] _actors;
        public LevelObject[] _levelObjects;
        public Transform[] _playerSpawns;
    }
}