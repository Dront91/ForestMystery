using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MysteryForest
{
    [CreateAssetMenu(fileName = "New DropList", menuName = "Entities/EnemyDropList", order = 51)]
    public class EnemyInventoryAsset : EntityAsset
    {
        [SerializeField] private List<Entity> _dropList;
        public List<Entity> DropList => _dropList;
    }
}
