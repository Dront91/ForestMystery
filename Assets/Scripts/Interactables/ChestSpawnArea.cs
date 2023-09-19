using TowerDefense;
using UnityEngine;
namespace MysteryForest
{
    public class ChestSpawnArea : CircleArea
    {
        public void MoveAwayFromPlayer(Vector3 newSpawnPosition)
        {
            transform.localPosition = -newSpawnPosition;
        }
    }
}
