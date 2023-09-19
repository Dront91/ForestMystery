using UnityEngine;

namespace MysteryForest
{
    public class MineBomb : Entity
    {
        private CircleCollider2D _triggerZone;
        private BombAsset _bomb;
        private bool _isExploded = false;
        private bool _isPlayerInside = false;
        private SoundHook _soundHook;

        [SerializeField] private GameObject _animExplode;

        private void Awake()
        {
            _bomb = (BombAsset)EntityAsset;
            _triggerZone = GetComponent<CircleCollider2D>();
            _soundHook = GetComponentInChildren<SoundHook>();
            _triggerZone.radius = _bomb.ExplosionRadius;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerController>())
            {
                _isPlayerInside = true;
            }
            else if (collision.GetComponent<Destructible>())
            {
                Invoke(nameof(Explode), _bomb.ExplosionTimer);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerController>())
            {
                _isPlayerInside = false;
            }
        }

        private void Explode()
        {
            if (_isExploded || !_isPlayerInside)
            {
                return;
            }

            _isExploded = true;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _bomb.ExplosionRadius);

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out Destructible target))
                {
                    if (target.TryGetComponent(out PlayerController player))
                    {
                        continue;
                    }

                    ApplyDamage(target);
                }
            }

            //Instantiate(_bomb.ExplosionVisualEffect);
            Instantiate(_animExplode, transform.position, Quaternion.identity);
            _soundHook.Play();
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