using UnityEngine;
namespace MysteryForest
{
    [CreateAssetMenu(fileName = "New Bomb", menuName = "Entities/Bomb", order = 51)]
    public class BombAsset : DropFromChestAsset
    {
        [Header("Bomb Settings")]
        [Range(0, 1)]
        [SerializeField] private float _maxDamage;
        [Range(0, 1)]
        [SerializeField] private float _minDamage;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _explosionTimer;
        [SerializeField] private float _pushForce;
        [SerializeField] private GameObject _explosionVisualEffect;
        [SerializeField] private GameObject _bombProjectile;

        public float MaxDamage => _maxDamage;
        public float MinDamage => _minDamage;
        public float ExplosionRadius => _explosionRadius;
        public float ExplosionTimer => _explosionTimer;
        public float PushForce => _pushForce;
        public GameObject ExplosionVisualEffect => _explosionVisualEffect;
        public GameObject BombProjectile => _bombProjectile;
    }
}
