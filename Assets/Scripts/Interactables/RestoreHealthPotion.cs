using UnityEngine;


namespace MysteryForest
{
    public class RestoreHealthPotion : Entity
    {
        protected int _capacity;

        public override void Start()
        {
            base.Start();
            ApplyPreferences(EntityAsset);
        }
        public override void ApplyPreferences(EntityAsset asset)
        {
            base.ApplyPreferences(asset);
            if (asset is PotionAsset)
            {
                _capacity = (asset as PotionAsset).Capacity;
            }
            else
            {
                Debug.Log("Incorrect type of EntityAsset set, check it!");
                return;
            }

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.TryGetComponent(out PlayerController player);

            if (!player)
                return;
            if (EntityAsset is PotionAsset)
            {
                player.GetComponent<PlayerFighter>().RestoreHitPoints(_capacity);
            }
            else
            {
                Debug.Log("Incorrect type of EntityAsset set, check it!");
                return;
            }
        }
    }
}
