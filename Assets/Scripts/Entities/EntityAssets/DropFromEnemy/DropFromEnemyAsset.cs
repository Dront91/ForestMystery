using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MysteryForest
{
    public class DropFromEnemyAsset : EntityAsset
    {
        [Range(0.0f, 1.0f)]
        public float DropChanceFromMole;
        [Range(0.0f, 1.0f)]
        public float DropChanceFromSquirrel;
        [Range(0.0f, 1.0f)]
        public float DropChanceFromHedgehog;
        [Range(0.0f, 1.0f)]
        public float DropChanceFromHornet;
        [Range(0.0f, 1.0f)]
        public float DropChanceFromRabbit;
        [Range(0.0f, 1.0f)]
        public float DropChanceFromHive;
        [Range(0.0f, 1.0f)]
        public float DropChanceFromSnakeBoss;
    }
}
