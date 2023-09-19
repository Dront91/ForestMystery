using UnityEngine;

namespace MysteryForest
{
    public class BombDrop : Entity
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.TryGetComponent(out PlayerInventory inventory);

            if (!inventory)
                return;

            inventory.AddBomb(_entityAsset as BombAsset, 1);
        }
    }
}
