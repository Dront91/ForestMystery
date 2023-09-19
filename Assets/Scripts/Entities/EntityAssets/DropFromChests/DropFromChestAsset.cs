using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MysteryForest
{
    public class DropFromChestAsset : EntityAsset
    {
        [Range(0.0f, 1.0f)]
        public float DropChanceFromCommonChest;
        [Range(0.0f, 1.0f)]
        public float DropChanceFromUmcommonChest;
        [Range(0.0f, 1.0f)]
        public float DropChanceFromRareChest;
        [Range(0.0f, 1.0f)]
        public float DropChanceFromEpicChest;
        [Range(0.0f, 1.0f)]
        public float DropChanceFromLegendaryChest;
        [Range(0, 100)]
        public int ItemValue;
    }
}
