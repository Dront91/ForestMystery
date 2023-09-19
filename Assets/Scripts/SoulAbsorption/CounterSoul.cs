using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MysteryForest
{
    public class CounterSoul : MonoBehaviour
    {
        [SerializeField] protected int _soulCounter = 0;

        public int SoulCounter => _soulCounter;

        public Text QuantitySoul;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out SoulNumber soulNumber))
            {
                _soulCounter += soulNumber.NumberSoul;
                Destroy(collision.gameObject);
                UpdateText();
            }
        }

        public void UpdateText()
        {
            if (QuantitySoul == null)
                return;

            QuantitySoul.text = _soulCounter.ToString();
        }

    }
}