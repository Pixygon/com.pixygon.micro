using Pixygon.Effects;
using UnityEngine;

namespace Pixygon.Micro {
    public class ScoreManager : MonoBehaviour {
        private LevelLoader _levelLoader;
        private int _points;
        private int _savedPoints;

        public void Initialize(LevelLoader loader) {
            _levelLoader = loader;
            ResetPoints();
        }
        
        public void AddPoints(int points, Vector3 pos) {
            _points += points;
            _levelLoader.Ui.SetPoints(_points);
            EffectsManager.SpawnScoreEffect(points, pos);
        }

        public void SetSavedPoints() {
            _savedPoints = _points;
        }

        public void ResetPointsToSaved() {
            _points = _savedPoints;
            _levelLoader.Ui.SetPoints(_points);
        }

        private void ResetPoints() {
            _points = 0;
            _levelLoader.Ui.SetPoints(_points);
        }
    }
}