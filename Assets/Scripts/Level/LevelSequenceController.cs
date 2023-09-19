using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Zenject;

namespace MysteryForest
{
    public class LevelSequenceController : MonoBehaviour
    {
        [SerializeField] private Level[] _levels;
        public Level[] Levels => _levels;

        private Level _currentLevel;
        public Level CurrentLevel => _currentLevel;
        public UnityAction OnMainMenuLoaded;

        public Action OnPrologueStart;
        public Action OnFinishPrologueStart;
        public Action OnRestart;

        [Inject] private AccountDataSaver _accountDataSaver;

        public void LoadMainMenu()
        {
            _currentLevel = null;
            OnMainMenuLoaded?.Invoke();
            SceneManager.LoadScene(0);
        }

        public void LoadLevel(Level level)
        {
            _currentLevel = level;
            _accountDataSaver.AddAttempt(_currentLevel);
            SceneManager.LoadScene(_currentLevel.LevelName);
            
        }

        public void LoadNextLevel()
        {
            if (_currentLevel == null || _levels.Length == 0) return;

            for (int i = 0; i < _levels.Length; i++)
            {
                if (_levels[i] == _currentLevel)
                {
                    if (i == _levels.Length - 1)
                    {
                        LoadMainMenu();
                        break;
                    }

                    LoadLevel(_levels[i + 1]);
                    break;
                }
            }
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            _accountDataSaver.AddAttempt(_currentLevel);
            OnRestart?.Invoke();
        }

        public void InvokePlayPrologue()
        {
            OnPrologueStart?.Invoke();
        }

        public void InvokePlayFinishPrologue()
        {
            OnFinishPrologueStart?.Invoke();
        }

        public void InvokeLevelComplete()
        {
            _accountDataSaver.AddWinAttempt(_currentLevel);
        }

        public void InvokeLoseLevel()
        {
            _accountDataSaver.AddLoseAttempt(_currentLevel);
        }
    }
}