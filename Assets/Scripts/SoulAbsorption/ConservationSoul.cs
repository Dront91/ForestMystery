using UnityEngine;
using Zenject;

namespace MysteryForest
{
    public class ConservationSoul : CounterSoul
    {
        [Inject] private LevelSequenceController _LevelSequenceController;
        void Start()
        {
            _LevelSequenceController.OnPrologueStart += ResetValue; //LevelSequenceController

            _soulCounter = PlayerPrefs.GetInt("_soul", 0);
        }
        private void OnDestroy()
        {
            _LevelSequenceController.OnPrologueStart -= ResetValue;

            PlayerPrefs.SetInt("_soul", _soulCounter);
            PlayerPrefs.Save();

            //PlayerPrefs.DeleteAll();
        }
        private void ResetValue()
        {
            PlayerPrefs.DeleteKey("_soul");
        }
        //void Start()
        //{
        //    _soulCounter = PlayerPrefs.GetInt("_soul", 0);
        //}
        //private void OnDestroy()
        //{
        //    PlayerPrefs.SetInt("_soul", _soulCounter);
        //    PlayerPrefs.Save();

        //    //PlayerPrefs.DeleteAll();
        //}

    }
}