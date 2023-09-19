using UnityEngine;
using UnityEngine.UI;

namespace MysteryForest
{
    public class UIHitPoints : MonoBehaviour
    {
        [SerializeField] private Destructible _destructible;
        [SerializeField] private Image _hitPoints;

        private void Update()
        {
            _hitPoints.fillAmount = Mathf.InverseLerp(0f, _destructible.MaxHitPoints, _destructible.CurrentHitPoints);
        }
    }
}