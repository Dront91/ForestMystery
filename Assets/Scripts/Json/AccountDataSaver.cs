using System;
using System.Collections.Generic;
using UnityEngine;
namespace MysteryForest
{
    public class AccountDataSaver : MonoBehaviour
    {
        [Serializable]
        private class LevelData
        {
            public string _levelName;
            public int _attemptCount;
            public int _winAttemptCount;
            public int _loseAttemptCount;

           public LevelData(string name) 
           { 
                _levelName = name; 
                _attemptCount = 0; 
                _winAttemptCount = 0; 
                _loseAttemptCount = 0; 
           }
        }
        public const string filename = "Level_data.dat";
        [SerializeField] private List<LevelData> _levelData;

        private void Awake()
        {
            Saver<List<LevelData>>.TryLoad(filename, ref _levelData);
        }
        public void AddWinAttempt(Level _currentLevel)
        {
            foreach(var level in _levelData)
            {
                if(_currentLevel.LevelName == level._levelName)
                {
                    level._winAttemptCount++;
                }
            }
            Save();
        }
        public void AddLoseAttempt(Level _currentLevel)
        {
            foreach (var level in _levelData)
            {
                if (_currentLevel.LevelName == level._levelName)
                {
                    level._loseAttemptCount++;
                }
            }
            Save();
        }
        public void AddAttempt(Level _currentLevel)
        {
            CheckAndAddNewLevel(_currentLevel);
            foreach (var level in _levelData)
            {
                if (_currentLevel.LevelName == level._levelName)
                {
                    level._attemptCount++;
                }
            }
            Save();
        }
        private void CheckAndAddNewLevel(Level _currentLevel)
        {

            foreach (var level in _levelData)
            {
                if(_currentLevel.LevelName == level._levelName) 
                 return; 
            }
            LevelData D = new LevelData(_currentLevel.LevelName);
            _levelData.Add(D); 
        }
        private void Save()
        {
            Saver<List<LevelData>>.Save(filename, _levelData);
        }
    }
}
