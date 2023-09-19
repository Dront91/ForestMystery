using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MysteryForest
{
    public class UIShowPassiveItems : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        [SerializeField] private PassiveItemView _passiveItemViewPrefab;

        public void InstantiatePassivItem(TestPassiveItem sourcePassiveItem)
        {
            var view = Instantiate(_passiveItemViewPrefab, _content.transform);
            view.Init(sourcePassiveItem.SpriteRenderer.sprite);
        }
    }

}

