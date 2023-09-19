using UnityEngine;

namespace MysteryForest
{
    public class IncreaseHealthPotion : RestoreHealthPotion
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.TryGetComponent(out PlayerController player);

            if (!player)
                return;
            if (EntityAsset is PotionAsset)
            {
                player.GetComponent<PlayerFighter>().IncreaseMaxHitPoints(_capacity);
            }
            else
            {
                Debug.Log("Incorrect type of EntityAsset set, check it!");
                return;
            }
        }
    }
}
