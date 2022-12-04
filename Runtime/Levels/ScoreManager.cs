using Pixygon.Effects;
using UnityEngine;

namespace Pixygon.Micro {
    public class ScoreManager : MonoBehaviour {
        private LevelLoader _levelLoader;
        private int _points;

        public void Initialize(LevelLoader loader) {
            _levelLoader = loader;
            ResetPoints();
        }
        
        private void SetPoints(int points) {
            _points += points;
            _levelLoader.Ui.SetPoints(_points);
            EffectsManager.SpawnScoreEffect(points, transform.position);
        }

        private void ResetPoints() {
            _points = 0;
            _levelLoader.Ui.SetPoints(_points);
        }
    }
}