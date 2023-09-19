using UnityEngine;

namespace MysteryForest
{
    public class BlastBomb : Entity
    {
        [SerializeField] private GameObject _animExplode;

        private BombAsset _bomb;
        private bool exploded = false;
        private SoundHook _soundHook;

        private void Awake()
        {
            _bomb = (BombAsset)EntityAsset;
            _soundHook = GetComponentInChildren<SoundHook>();
            Invoke(nameof(Explode), _bomb.ExplosionTimer);
        }

        private void Explode()
        {
            if (exploded)
                return;

            exploded = true;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _bomb.ExplosionRadius);
            Destructible closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out Destructible target))
                {
                    
                    if (target.TryGetComponent(out PlayerController player))
                    {
                        ApplyDamage(target);
                    }
                    else
                    {
                        float distance = Vector2.Distance(transform.position, target.transform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestTarget = target;
                        }
                    }
                }
            }

            if (closestTarget != null)
            {
                ApplyDamage(closestTarget);
            }
            _soundHook.Play();

            Instantiate(_animExplode, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private void ApplyDamage(Destructible target)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            float damageDistanceScale = Mathf.Clamp01(1 - (distance / _bomb.ExplosionRadius));
            float scaledDamage = Mathf.Lerp(_bomb.MinDamage, _bomb.MaxDamage, damageDistanceScale);
            float finalDamage = target.MaxHitPoints * scaledDamage;
            target.TakeDamage((int)finalDamage, _bomb.PushForce, transform.position);
        }
    }
}
