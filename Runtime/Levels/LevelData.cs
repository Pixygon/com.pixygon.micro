using Pixygon.Micro.Parallax;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Pixygon.Micro
{
    [CreateAssetMenu(menuName = "Pixygon/Micro/New LevelData")]
    public class LevelData : ScriptableObject {
        public string _levelName;
        public string _levelId;
        public MicroActorData _playerOverride;
        public AssetReference _levelRef;
        public AssetReference _bgmRef;
        public AssetReference _levelClearSfxRef;
        public AssetReference _postProcessingProfileRef;
        
        public bool _useParallax;
        public ParallaxLayerData[] _parallaxLayerDatas;
    }
}
