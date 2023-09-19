using UnityEngine;

namespace MysteryForest
{
    public class UISlideSwitcher : MonoBehaviour
    {
        [SerializeField] GameObject _nextSlide;
        [SerializeField] private bool _isClose = true;

        public void SwitchSlide()
        {
            if (!_nextSlide)
                return;
            if(_isClose)
                this.gameObject.SetActive(false);
            _nextSlide.SetActive(true);
        }
    }
}
