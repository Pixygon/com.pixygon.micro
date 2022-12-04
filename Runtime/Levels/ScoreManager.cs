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
        
        public void AddPoints(int points) {
            _points += points;
            _levelLoader.Ui.SetPoints(_points);
            EffectsManager.SpawnScoreEffect(points, transform.position);
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