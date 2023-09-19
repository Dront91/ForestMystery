using System.Collections;
using UnityEngine;

namespace MysteryForest
{
    public class AttackPoint : MonoBehaviour
    {
        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _attackDistance;
        private Fighter _fighter;
        
        public float AttackDistance => _attackDistance;        
        Destructible _target;

        private void Awake()
        {
            _fighter = GetComponentInParent<Fighter>();
        }
        private int Crete(int damage)
        {
            if(Random.Range(0,101) <= _fighter.CreteChance)
            {               
                return (int)(damage * _fighter.CriticalDamageCoef);
            }            
            return damage;
        }

        public void MeleeAttack()
        {
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + transform.right * _attackDistance, _attackRadius, _targetMask);
                
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent(out _target))
                    {
                        _target.TakeDamage(Crete(_fighter.Damage), _fighter.PushForce, transform.position);
                    }
                }
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.right * _attackDistance, _attackRadius);
        }
#endif     
    }

}