using UnityEngine;
namespace MysteryForest
{
    public class EnvironmentSprite : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private float _offset = 0.7f;
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out PlayerController p))
            {
                if (collision.transform.position.y - _offset > transform.position.y)
                {
                    _spriteRenderer.sortingOrder = 0;
                }
                else
                {
                    _spriteRenderer.sortingOrder = -1;
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out PlayerController p))
            {
                _spriteRenderer.sortingOrder = -1;
            }
        }
    }
}
