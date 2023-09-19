using UnityEngine;

namespace MysteryForest
{
    [RequireComponent(typeof(Collider2D))]
    public class SnakeHead : MonoBehaviour
    {
        private CircleCollider2D _collider;
        private AISnake _snakeAI;

        private int _damage;
        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();   
            _snakeAI = GetComponentInParent<AISnake>();
        }

        private void Start()
        {
            _collider.enabled = false;

            if(_snakeAI == null)
            {
                Debug.Log("No SnakeAI in parent.");
                return;
            }

            _snakeAI.OnRushAttackStart += CalculateCollisinActive;
            _snakeAI.OnRushAttackEnd += CalculateCollisinDisactive;
        }

        private void OnDestroy()
        {
            _snakeAI.OnRushAttackStart -= CalculateCollisinActive;
            _snakeAI.OnRushAttackEnd -= CalculateCollisinDisactive;
        }

        private void CalculateCollisinActive(int damage)
        {
            _collider.enabled = true;

            _damage = damage;   
        }
        private void CalculateCollisinDisactive()
        {
            _collider.enabled = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent(out Destructible target))                         
                target.TakeDamage(_damage, _snakeAI.RushPushForce, transform.position);           
        }
    }
}