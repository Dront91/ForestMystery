using TowerDefense;
using UnityEngine;

namespace MysteryForest
{
    public class SpawnSoul : MonoBehaviour
    {
        [SerializeField] private GameObject _soul;
        [SerializeField] private CircleArea _spawnArea;

        public void Spawn(int CountSoul)
        {
            if (CountSoul == 0) return;
            GameObject game = Instantiate(_soul,_spawnArea.GetRandomPointInsideCircle() ,Quaternion.identity);
            game.GetComponent<SoulNumber>().NumberSoul = CountSoul;
        }
    }
}