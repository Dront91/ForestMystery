using UnityEngine;
using Zenject;
namespace MysteryForest
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class OwlStandPoint : MonoBehaviour
    {
        [SerializeField] private float _colliderRadius;
        private RoomController _roomController;
        [Inject] private AIOwl _owl;
        private CircleCollider2D _collider;
        private bool _isActive;
        public bool _isOwlOnPoint;
        private void Start()
        {
            _roomController = GetComponentInParent<RoomController>();
            _collider = GetComponent<CircleCollider2D>();
            _collider.isTrigger = true;
            _collider.radius = _colliderRadius;
        }
        private void Update()
        {
            if (_roomController.IsActive == true)
            {
                _isActive = true;
                if (Vector2.Distance(transform.position, _owl.transform.position) < _colliderRadius)
                    _isOwlOnPoint = true;
                else
                    _isOwlOnPoint = false;
            }
            else
                _isActive = false;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isActive == false || _isOwlOnPoint == false) return;
            if(collision.GetComponent<PlayerController>() != null && _owl != null)
            {
                _owl.FlyAwayFromPlayer(collision.transform.position);
            }
        }

#if UNITY_EDITOR
        private static Color gizmoColor = new(1, 0, 0, 0.15f);

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, _colliderRadius);
        }
#endif
    }
}
