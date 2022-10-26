using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Pixygon.Micro
{
    [CreateAssetMenu(menuName = "Pixygon/Micro/New LevelData")]
    public class LevelData : ScriptableObject {
        public string _levelName;
        public AssetReference _levelRef;
        public AssetReference _bgmRef;
        public AssetReference _postProcessingProfileRef;
        public ParallaxLayerData[] _parallaxLayerDatas;
    }
}