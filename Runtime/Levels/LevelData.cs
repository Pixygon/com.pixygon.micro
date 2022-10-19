using UnityEngine;

namespace Pixygon.Micro
{
    [CreateAssetMenu(menuName = "Pixygon/Micro/New LevelData")]
    public class LevelData : ScriptableObject {
        public GameObject _levelPrefab;
        public ParallaxLayerData[] _parallaxLayerDatas;
        public AudioClip _bgm;
    }
}