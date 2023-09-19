using UnityEngine;

namespace MysteryForest
{
    public class TestPassiveItem : Entity
    {
        public passivItems _passivItems;
        public enum passivItems
        {
            Chips, 
            Berry, 
            RedSock,
            BlueSock,
            GearWheel,
            Drink,
            Headphones,
            Bracelet,
            Midge,
            Hummingbirds
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.TryGetComponent(out PlayerInventory inventory);

            if (!inventory)
                return;

            inventory.AddPassiveItem((PassiveItemAsset)this.EntityAsset);
        }
    }
}
