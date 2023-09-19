using System.Collections;
using UnityEngine;
using Zenject;

namespace MysteryForest
{
    public class CoctailBomb : Entity
    {
        [SerializeField] private int _throwForce;
        [SerializeField] private int _tickInterval = 1;
        [SerializeField] private float _maxDistance = 4f;
        [SerializeField] private GameObject _animExplode;


        [Inject] private PlayerController _playerController;
        private BombAsset _bomb;
        private bool exploded = false;

        private Vector2 _throwDirection;
        private Rigidbody2D _rigidbody;
        private Vector2 _startingPosition;
        private SoundHook _soundHook;

        private void Awake()
        {
            _bomb = (BombAsset)EntityAsset;
            _rigidbody = GetComponent<Rigidbody2D>();
            _soundHook = GetComponentInChildren<SoundHook>();
            _throwDirection = CheckDirection();
            _startingPosition = transform.position;
            Throw();
        }

        private void FixedUpdate()
        {
            CheckDistance();
        }

        private void CheckDistance()
        {
            if (Vector2.Distance(transform.position, _startingPosition) <= _maxDistance)
            {
                return;
            }

            Instantiate(_animExplode, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.GetComponent<PlayerController>())
            {
                if (collision.TryGetComponent(out Destructible target))
                    Explode(target);
                else if (!collision.isTrigger)
                {
                    Instantiate(_animExplode, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                   
            }

        }

        private void Explode(Destructible target)
        {
            if (exploded)
                return;

            exploded = true;
            _soundHook.Play();

            Instantiate(_animExplode, transform.position, Quaternion.identity);
            Destroy(gameObject);

            _rigidbody.velocity = Vector2.zero;
            StartCoroutine(DamageOverTime(target));

            GetComponent<CircleCollider2D>().enabled = false;
            SpriteRenderer.enabled = false;
        }

        private IEnumerator DamageOverTime(Destructible target)
        {
            float elapsed = 0f;
            while (elapsed < _bomb.ExplosionTimer)
            {
                float tickDamage = Random.Range(_bomb.MinDamage, _bomb.MaxDamage);
                float finalDamage = target.MaxHitPoints * tickDamage;
                target.TakeDamage((int)finalDamage, 0 , transform.position);

                yield return new WaitForSeconds(_tickInterval);
                elapsed += _tickInterval;
            }
        }

        public void Throw()
        {
            _rigidbody.AddForce(_throwDirection * _throwForce, ForceMode2D.Impulse);
        }
        private Vector2 CheckDirection()
        {
            var _direction = Vector2.zero;
            switch(_playerController.PlayerAnimationController.CurrentDirection)
            {
                case PlayerAnimationController.PlayerDirection.Right:
                    _direction = Vector2.right;
                    break;
                case PlayerAnimationController.PlayerDirection.Left:
                    _direction = Vector2.left;
                    break;
                case PlayerAnimationController.PlayerDirection.Up:
                    _direction = Vector2.up;
                    break;
                case PlayerAnimationController.PlayerDirection.Down:
                    _direction = Vector2.down;
                    break;
            }
            return _direction;
        }
        private Vector2 CalculateThrowDirection()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - (Vector2)transform.position;
            direction.Normalize();
            return direction;
        }
    }
}
