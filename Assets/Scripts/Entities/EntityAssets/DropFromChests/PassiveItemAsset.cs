using UnityEngine;

namespace MysteryForest
{
    [CreateAssetMenu(fileName = "New Passive Item", menuName = "Entities/Passive Item", order = 51)]
    public class PassiveItemAsset : DropFromChestAsset
    {
        [SerializeField] private int _maxHealthIncrease;
        [SerializeField] private float _damageIncrease;
        [SerializeField] private float _attackRangeIncrease;

        public int MaxHealthIncrease => _maxHealthIncrease;
        public float DamageIncrease => _damageIncrease;
        public float AttackRangeIncrease => _attackRangeIncrease;
    }
}
