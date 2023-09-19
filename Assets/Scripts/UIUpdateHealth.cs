using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MysteryForest
{
    public class UIUpdateHealth : MonoBehaviour
    {
        [SerializeField] private Image _hitPoints;
        [SerializeField] private GameObject _imageDie;

        [Inject] private LevelSequenceController _levelSequenceController;

        // число на которое мы делим макс хп (50% - это желтый цвет, 25% - красный)
        private int _scaler = 2; 

        private void Start()
        {
            StartLevel();
            _levelSequenceController.OnRestart += StartLevel;
            _levelSequenceController.OnMainMenuLoaded += StartLevel;
        }

        private void OnDestroy()
        {
            _levelSequenceController.OnRestart -= StartLevel;
            _levelSequenceController.OnMainMenuLoaded -= StartLevel;
        }

        private void StartLevel()
        {
            _hitPoints.color = Color.green;
            _imageDie.SetActive(false);
        }

        public void UpdateHealth(float _maxHP, float _currentHP)
        {
            _hitPoints.fillAmount = Mathf.InverseLerp(0f, _maxHP, _currentHP);

            if (_currentHP > _maxHP / _scaler)
            {
                _hitPoints.color = Color.green;
            }

            if (_currentHP <= _maxHP / _scaler && _currentHP >= _maxHP / (_scaler * 2))
            {
                _hitPoints.color = Color.yellow;
            }

            if (_currentHP < _maxHP / (_scaler * 2))
            {
                _hitPoints.color = Color.red;
            }

            if (_currentHP <= 0)
            {
                _hitPoints.fillAmount = 1;
                _imageDie.SetActive(true);
                _hitPoints.color = new Color32(153, 85, 204,255);
            }
        }
    }
}

