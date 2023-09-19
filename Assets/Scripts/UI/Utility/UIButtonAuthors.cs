using UnityEngine;

namespace MysteryForest
{
    public class UIButtonAuthors : MonoBehaviour
    {
        [SerializeField] private float _speed = 1.5f;

        private float _time;

        private float _timeReset = 20f;

        private void Update()
        {
            _time = _time + Time.deltaTime;

            transform.position += _speed * Time.deltaTime * Vector3.down;

            if (_time >= _timeReset)
                StartPos();
        }
        public void StartPos()
        {
            transform.position = Vector3.zero;
            _time = 0;
        }
    }
}
