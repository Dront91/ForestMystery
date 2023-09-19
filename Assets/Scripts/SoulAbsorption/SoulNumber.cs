using UnityEngine;

namespace MysteryForest
{
    public class SoulNumber : MonoBehaviour
    {
        [SerializeField] private int _numberSoul;

        public int NumberSoul { 
            get
            {
                return _numberSoul;
            }
            set 
            {
                _numberSoul = value;
            }
        }
    }
}