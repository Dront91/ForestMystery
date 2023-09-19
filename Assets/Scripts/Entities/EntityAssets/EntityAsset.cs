using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MysteryForest
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public class EntityAsset : ScriptableObject
    {
        [SerializeField] private Rarity _rarity;
        [SerializeField] private Sprite _sprite;
        [Range(0.0f, 1.0f)]
        protected float _dropChance = 0f;
        public float DropChance => _dropChance;
        public Rarity Rarity => _rarity;
        public Sprite Sprite => _sprite;
        private Rarity _currentRarity;
       
    }
}

