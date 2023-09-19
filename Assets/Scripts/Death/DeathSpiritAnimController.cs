using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace MysteryForest
{
    [RequireComponent(typeof(Destructible))]
    public class DeathSpiritAnimController : MonoBehaviour
    {
        [SerializeField] private GameObject _deathSpiritAnimPrefab;

        private Destructible _destructible;
        private GameObject _objectInstance;

        private void Awake()
        {
            _destructible = GetComponent<Destructible>();
        }

        private void Start()
        {
            _destructible.OnDeath += OnDeathAnimActivation;
        }

        private void OnDeathAnimActivation(Destructible destructible)
        {
            if (_deathSpiritAnimPrefab == null)
            {
                Debug.LogWarning("Death animation prefab is missing");
                return;
            }

            _objectInstance = Instantiate(_deathSpiritAnimPrefab, transform.position, Quaternion.identity);

           // _objectInstance.transform.localScale = new Vector3(_scaleAnim, _scaleAnim);
        }

        private void OnDestroy()
        {
            _destructible.OnDeath -= OnDeathAnimActivation;
        }
    }
}


