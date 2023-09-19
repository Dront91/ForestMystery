using UnityEngine;

namespace MysteryForest
{
    public class Key : Entity
    {
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerInventory player)) 
                player.AddKeys(1);
        }
    }
}
