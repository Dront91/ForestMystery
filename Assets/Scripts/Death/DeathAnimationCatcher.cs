using UnityEngine;

namespace MysteryForest
{
    public class DeathAnimationCatcher : MonoBehaviour
    {
        public void EndDeathAnimation()
        {
            Destroy(gameObject);
        }
    }
}