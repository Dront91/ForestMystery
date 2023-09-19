
using UnityEngine;
namespace MysteryForest
{
    public class TreeSprite : MonoBehaviour
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
                    _spriteRenderer.color = new Color(255, 255, 255, 0.4f);
                }
                else
                {
                    _spriteRenderer.color = new Color(255, 255, 255, 1);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out PlayerController p))
            {
                _spriteRenderer.color = new Color(255, 255, 255, 1);
            }
        }
    }
}
