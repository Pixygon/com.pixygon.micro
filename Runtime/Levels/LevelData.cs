using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;

namespace Pixygon.Micro
{
    [CreateAssetMenu(menuName = "Pixygon/Micro/New LevelData")]
    public class LevelData : ScriptableObject {
        //public GameObject _levelPrefab;
        public AssetReference _levelRef;
        public AssetReference _bgmRef;
        public AssetReference _postProcessingProfileRef;
        public ParallaxLayerData[] _parallaxLayerDatas;
    }
}