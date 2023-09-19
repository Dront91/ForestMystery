using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MysteryForest
{
    [RequireComponent(typeof(Destructible))]
    public class DeathAnimController : MonoBehaviour
    {
        [SerializeField] private GameObject _deathAnimPrefab;
        [SerializeField] private Color _color;
        [SerializeField] private float _scaleAnim = 1f;

        private Destructible _destructible;
        private GameObject _objectInstance;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _destructible = GetComponent<Destructible>();
        }

        private void Start()
        {
            _destructible.OnDeath += OnDeathAnimActivation;
        }

        private void OnDeathAnimActivation(Destructible arg0)
        {
            if (_deathAnimPrefab == null)
            {
                Debug.LogWarning("Death animation prefab is missing");
                return;
            }

            _objectInstance = Instantiate(_deathAnimPrefab, transform.position, Quaternion.identity);
            
            if(_objectInstance.TryGetComponent(out _spriteRenderer))
                _spriteRenderer.color = _color;

            _objectInstance.transform.localScale = new Vector3(_scaleAnim, _scaleAnim);
        }

        private void OnDestroy()
        {
            _destructible.OnDeath -= OnDeathAnimActivation;
        }
    }
}