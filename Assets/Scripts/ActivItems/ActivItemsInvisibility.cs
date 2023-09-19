using UnityEngine;

namespace MysteryForest
{
    public class ActivItemsInvisibility : MonoBehaviour
    {
        

        [SerializeField] private float _moveSpeed = 3;
        [SerializeField] private float _maxMoveSpeed = 2;

        public float MoveSpeed
        {
            get
            {
                return _moveSpeed;
            }
            set
            {
                _moveSpeed = value;
            }
        }
        public float MaxMoveSpeed
        {
            get
            {
                return _maxMoveSpeed;
            }
            set
            {
                _maxMoveSpeed = value;
            }
        }
    }
}
