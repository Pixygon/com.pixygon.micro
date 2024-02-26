using System;
using UnityEngine;

namespace Pixygon.Micro {
    [CreateAssetMenu(menuName = "Pixygon/MissionData")]
    public class MissionData : ScriptableObject {
        public string Id;
        public string MissionTitle;
        public string MissionDescription;
        public string MissionIssuer;
        public int MissionDifficulty;
        public int LevelId;
        public int MissionId;
        public int Reward;
        public string FullMissionId => $"{LevelId:02}{MissionId:04}";
        public MissionRequirements _missionRequirements;
    }

    [Serializable]
    public class MissionRequirements {
        public int Level;
        public int Cost;
        public string[] BeatenMissions;
    }
}