using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MysteryForest
{
    [CreateAssetMenu]
    public class Level : ScriptableObject
    {
        [SerializeField] private string _levelName;
        public string LevelName => _levelName;

        [SerializeField] private Sprite _previewImage;
        public Sprite PreviewImage => _previewImage;

        [SerializeField] private int _rewardCoin;
        public int RewardCoin => _rewardCoin;
        [SerializeField] private int _maxCommonChestCount;
        public int MaxCommonChestCount => _maxCommonChestCount;
        [SerializeField] private int _maxUmcommonChestCount;
        public int MaxUmcommonChestCount => _maxUmcommonChestCount;
        [SerializeField] private int _maxRareChestCount;
        public int MaxRareChestCount => _maxRareChestCount;
        [SerializeField] private int _maxEpicChestCount;
        public int MaxEpicChestCount => _maxEpicChestCount;
        [SerializeField] private int _maxLegendaryChestCount;
        public int MaxLegendaryChestCount => _maxLegendaryChestCount;
        [SerializeField] private Sound _ambientSound;
        public Sound AmbientSound => _ambientSound;
    }
}
