using UnityEngine;

namespace MysteryForest
{
    public class PickUp : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.TryGetComponent(out PlayerController player);

            if (!player)
                return;

            Destroy(gameObject);
        }
    }

}

