using System;
using UnityEngine;

namespace MysteryForest
{
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private GameObject _impactEffectPrefab;
        [SerializeField] private int _damage;
        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private float _rotateSpeed = 1f;

        private float _rotationRatio = 100f;
        private Vector2 _target;
        private float _pushForce;
        private Destructible _destructible;
        protected SoundPlayer _soundPlayer;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Collider2D _parentCollider;
        private SoundHook _soundHook;

        virtual protected void Start()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _soundHook = GetComponent<SoundHook>();
        }
        virtual protected void Update()
        {
            MoveToTarget();
            Rotate();
            CheckDestinationReached();
        }
        virtual protected void MoveToTarget()
        {
            transform.Translate(_moveSpeed * Time.deltaTime * (_target - (Vector2)transform.position).normalized);
        }

        virtual protected void Rotate()
        {
            if (_spriteRenderer == null) return;

            _spriteRenderer.transform.Rotate(new Vector3(0, 0, _rotateSpeed * _rotationRatio * Time.deltaTime));
        }

        private void CheckDestinationReached()
        {
            if (Vector2.Distance(transform.position, _target) < 0.1f)
                OnProjectileLifeEnd(false);
        }

        public void SetDestination(Vector2 target)
        {
            _target = target;
        }

        public void SetParentCollider(Collider2D collider)
        {
            _parentCollider = collider;
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
        }
        public void SetPushForce(float pushForce)
        {
            _pushForce = pushForce;
        }
        public void SetSpriteDirection(float angle)
        {
            _spriteRenderer.transform.Rotate(0, 0, angle);
        }
        public void SetSoundPlayer(SoundPlayer player)
        {
            _soundPlayer = player;
        }

        private void OnProjectileLifeEnd(bool hit)
        {
            if(hit == true && _soundHook != null)
            {
                _soundPlayer.Play(_soundHook.m_Sound);
            }
            Instantiate(_impactEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_parentCollider == collision.transform.GetComponent<Collider2D>()) return;
            if (collision.transform.TryGetComponent(out _destructible))
            { 
                _destructible.TakeDamage(_damage, _pushForce, transform.position);
                OnProjectileLifeEnd(true);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_parentCollider == collision.transform.GetComponent<Collider2D>() || collision.transform.GetComponent<Destructible>() == null) return;
            if (collision.transform.TryGetComponent(out _destructible))
            {
                _destructible.TakeDamage(_damage, _pushForce, transform.position);
                OnProjectileLifeEnd(true);
            }
        }

    }
}