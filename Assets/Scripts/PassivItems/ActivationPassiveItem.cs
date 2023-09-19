using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MysteryForest
{
    public class ActivationPassiveItem : MonoBehaviour
    {
        public Image SpriteRenderer;

        private TestPassiveItem.passivItems _testPassive;
        public TestPassiveItem.passivItems PassiveItem => _testPassive;


        public void InstallImage(Sprite sprite, TestPassiveItem.passivItems testPassiveItem)
        {
            _testPassive = testPassiveItem;
            SpriteRenderer.sprite = sprite;
        }
    }
}


