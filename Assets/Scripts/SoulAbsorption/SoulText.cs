using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MysteryForest
{
    public class SoulText : MonoBehaviour
    {
        private void Update()
        {
            CounterSoul counterSoul = FindObjectOfType<CounterSoul>();

            if(counterSoul)
            {
                counterSoul.QuantitySoul = GetComponent<Text>();
                counterSoul.UpdateText();
                //Destroy(this);
            }
        }
    }
}