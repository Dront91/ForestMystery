using System.Collections.Generic;
using UnityEngine;
namespace MysteryForest
{
    [CreateAssetMenu(fileName = "New Chest", menuName = "Entities/Chest", order = 51)]
    public class ChestAsset : EntityAsset
    {
        
        [SerializeField] private Rarity _chestRarity;
        public Rarity ChestRarity => _chestRarity;
        [SerializeField] private List<Entity> _dropList;
        public List<Entity> DropList => _dropList;
        public int ChestCapacity;

    }
}