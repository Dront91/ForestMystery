using UnityEngine;

namespace TowerDefense
{
    public class CircleArea : MonoBehaviour
    {
        [SerializeField] private float _radius;
        public float Radius => _radius;

        public Vector2 GetRandomPointInsideCircle()
        {
            return (Vector2)transform.position + Random.insideUnitCircle * _radius;
        }

#if UNITY_EDITOR
        private static Color gizmoColor = new (1, 0, 0, 0.15f);

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, _radius);
        }
#endif
    }
}