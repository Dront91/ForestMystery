using UnityEngine;

namespace MysteryForest
{
    public class AIControllerBase : ActivItemsInvisibility
    {
        [SerializeField] protected float _npcSightDistance;
        public float NPCSightDistance => _npcSightDistance;
        [SerializeField] private int _enemyStrength;
        public int EnemyStrength => _enemyStrength;

        protected Collider2D _npcSight;
        protected LayerMask _targetLayerMask;
        protected Destructible _attackTarget;
        protected SpriteRenderer _spriteRenderer;

        [HideInInspector] public bool Invisibility;

        private float _tempNpcSightDistance;

        virtual protected void Awake()
        {
            _targetLayerMask = LayerMask.GetMask("Player");
            _spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        }

        public void AIEnable(bool state)
        {
            if(state)
            {
                if(_npcSightDistance == 0f)
                    _npcSightDistance = _tempNpcSightDistance;
            }
            else
            {
                _tempNpcSightDistance = _npcSightDistance;
                _npcSightDistance = 0f;
                _attackTarget = null;
            }
        }

        virtual protected void FindAttackTarget()
        {
            if (_attackTarget != null || _npcSightDistance == 0f)
                return;

            _npcSight = Physics2D.OverlapCircle(transform.position, _npcSightDistance, _targetLayerMask);

            if (_npcSight)
            {
                _npcSight.TryGetComponent(out _attackTarget);
            }
        }

        virtual protected void Flip()
        {

        }


#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _npcSightDistance);
        }
#endif
    }
}