using Pixygon.Effects;
using UnityEngine;

namespace Pixygon.Micro {
    public class ScoreManager : MonoBehaviour {
        private LevelLoader _levelLoader;

        public int Points { get; private set; }
        public int PointsTotal { get; private set; }
        public void Initialize(LevelLoader loader) {
            _levelLoader = loader;
            ResetPoints();
        }
        
        public void AddPoints(int points, Vector3 pos) {
            if (_levelLoader.Difficulty == 1) points *= 3;
            if (_levelLoader.Difficulty == 2) points *= 5;
            Points += points;
            _levelLoader.Ui.SetPoints(Points);
            EffectsManager.SpawnScoreEffect(points, pos);
        }

        public void SetSavedPoints() {
            PointsTotal = Points;
        }

        public void ResetPointsToSaved() {
            Points = PointsTotal;
            _levelLoader.Ui.SetPoints(Points);
        }

        private void ResetPoints() {
            Points = 0;
            _levelLoader.Ui.SetPoints(Points);
        }
    }
}