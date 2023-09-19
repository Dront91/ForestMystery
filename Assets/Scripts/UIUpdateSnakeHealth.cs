using Zenject;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace MysteryForest
{
    public class UIUpdateSnakeHealth : MonoBehaviour
    {
        [SerializeField] private Image _hitPoints;
        [SerializeField] private GameObject _sliderSnake;
        [SerializeField] private GameObject _snakeCards;

        public Action CloseCanvas;

        [Inject] private LevelSequenceController _levelSequenceController;

        private void Start()
        {
            StartLevel();
            _levelSequenceController.OnRestart += StartLevel;
        }

        private void OnDestroy()
        {
            _levelSequenceController.OnRestart -= StartLevel;
        }

        public void ShowSlider()
        {
            _snakeCards.SetActive(true);
            _sliderSnake.SetActive(true);
            _hitPoints.gameObject.SetActive(true);
        }

        public void CloseSnakeCanvas()
        {
            _snakeCards.SetActive(false);
            CloseCanvas?.Invoke();
        }

        private void StartLevel()
        {
            _hitPoints.gameObject.SetActive(false);
            _sliderSnake.SetActive(false);
        }

        public void UpdateHealthSnake(float _maxHP, float _currentHP)
        {
            _hitPoints.fillAmount = Mathf.InverseLerp(0f, _maxHP, _currentHP);
        }
    }
}

