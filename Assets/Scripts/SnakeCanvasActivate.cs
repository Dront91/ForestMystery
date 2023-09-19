using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MysteryForest
{
    public class SnakeCanvasActivate : MonoBehaviour
    {
        [Inject] private UICanvasManager _uICanvasManager;
        [SerializeField] private AISnake _AISnake;

        private UIUpdateSnakeHealth _uIUpdateSnakeHealth;

        private void Awake()
        {
            _uIUpdateSnakeHealth = _uICanvasManager.GetComponent<UIUpdateSnakeHealth>();
            _uIUpdateSnakeHealth.CloseCanvas += EnableRoomController;
        }

        private void OnDestroy()
        {
            _uIUpdateSnakeHealth.CloseCanvas -= EnableRoomController;
        }

        private void EnableRoomController()
        {
            _AISnake.enabled = true;
        }

    }
}

