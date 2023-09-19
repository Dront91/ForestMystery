using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MysteryForest
{
    [CreateAssetMenu(fileName = "New Potion", menuName = "Entities/Potion", order = 51)]
    public class PotionAsset : DropFromChestAsset
    {
        
        [SerializeField] private int _capacity;
        public int Capacity => _capacity;

        
    }
   
}
