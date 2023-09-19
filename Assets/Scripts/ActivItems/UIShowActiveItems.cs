using System.Collections.Generic;
using UnityEngine;

namespace MysteryForest
{
    public class UIShowActiveItems : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        [SerializeField] private ActivationActiveItems _activationActiveItemsPrefab;

        private List<ActivationActiveItems> _activationActiveItems = new List<ActivationActiveItems>();
        public List<ActivationActiveItems> ActivationActiveItems => _activationActiveItems;

        public void InstantiateActiveItems(ActivItemsPrefab activItemsPrefab)
        {
            if (!activItemsPrefab)
                return;

            ActivItem activItems = activItemsPrefab.ActivItem;

            ActivationActiveItems activationActiveItems = Instantiate(_activationActiveItemsPrefab);

            activationActiveItems.InstallImage(activItemsPrefab.SpriteRenderer.sprite, activItems);

            activationActiveItems.transform.SetParent(_content.transform);

            if (_activationActiveItems.Exists(x => x.ActivItem == activItems))
            {
                int index = _activationActiveItems.FindIndex(x => x.ActivItem == activItems);
                Destroy(_activationActiveItems[index].gameObject);
                _activationActiveItems.RemoveAt(index);
            }

            _activationActiveItems.Add(activationActiveItems);
        }
    }
}