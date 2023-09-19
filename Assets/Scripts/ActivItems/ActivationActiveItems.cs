using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MysteryForest
{
    public class ActivationActiveItems : MonoBehaviour
    {
        public Image SpriteRenderer;
        //private ActivItems _activItems;
        private ActivItem _activItem;
        public ActivItem ActivItem => _activItem;

        
        public void InstallImage(Sprite sprite,/* ActivItems activItems,*/ ActivItem activItem)
        {
            _activItem = activItem;
            //_activItems = activItems;
            SpriteRenderer.sprite = sprite;
        }
        //public void ClickItems()
        //{
        //    _activItems.ActivationActiveItems(_activItem);
        //}


    }
}