using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MysteryForest
{
    [CreateAssetMenu(fileName = "New Active Item", menuName = "Entities/Active Item", order = 51)]
    public class ActiveItemAsset : DropFromChestAsset
    {
        [SerializeField] private int _maxHealthIncrease;
        [SerializeField] private float _damageIncrease;
        [SerializeField] private float _attackRangeIncrease;

        public int MaxHealthIncrease => _maxHealthIncrease;
        public float DamageIncrease => _damageIncrease;
        public float AttackRangeIncrease => _attackRangeIncrease;
    }
}

