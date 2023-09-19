using System.Collections;
using UnityEngine;

namespace MysteryForest
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SnakeShadow : MonoBehaviour
    {
        [SerializeField] private float _elapseStep;

        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;
        private Vector3 _defaultScale;

        private const float _startAlpha = 0.5f;
        private const float _startScaleRatio = 0.1f;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _defaultColor = _spriteRenderer.color;
            _defaultScale = transform.localScale;

            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, 
                                              _spriteRenderer.color.b, _startAlpha);
            transform.localScale *= _startScaleRatio;

            StartCoroutine(ShadowElapse());
        }

        private IEnumerator ShadowElapse()
        {
            while (((Vector2)_defaultScale - (Vector2)transform.localScale).magnitude > 0.1f)
            {
                float step = Time.deltaTime * _elapseStep;
                transform.localScale = new Vector2(transform.localScale.x + step, transform.localScale.y + step);

                if (_spriteRenderer.color.a < _defaultColor.a)
                    _spriteRenderer.color += new Color(0, 0, 0, step);

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
