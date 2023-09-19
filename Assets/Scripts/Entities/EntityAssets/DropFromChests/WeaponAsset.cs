using UnityEngine;
namespace MysteryForest
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Entities/Weapon", order = 51)]
    public class WeaponAsset : DropFromChestAsset
    {
        public enum WeaponType
        {
            Stick,
            Bow, 
            Slingshot,
            Sword
        }
        [SerializeField] private int _damage;
        [SerializeField] private float _range;
        [SerializeField] private WeaponType _typeWeapon;

        public int Damage => _damage;
        public float Range => _range;
        public WeaponType TypeWeapon => _typeWeapon;
    }
}
