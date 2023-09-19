using System;
using System.Collections.Generic;
using UnityEngine;
namespace MysteryForest
{
    [CreateAssetMenu]
    public class PlayerAnimationAsset : ScriptableObject
    {
        [Serializable]
        public class MoveAnimationClips
        {
           [HideInInspector] public string name;
            public AnimationClip withoutWeaponClip;
            public AnimationClip withBowClip;
            public AnimationClip withSlingshotClip;
            public AnimationClip withSwordClip;
            public AnimationClip withStickClip;
        }
        [Serializable]
        public class IdleAnimationClips
        {
            [HideInInspector]public string name;
            public AnimationClip withoutWeaponClip;
            public AnimationClip withBowClip;
            public AnimationClip withSlingshotClip;
            public AnimationClip withSwordClip;
            public AnimationClip withStickClip;
        }
        [Serializable]
        public class WithoutWeaponAttackAnimationClips
        {
            [HideInInspector] public string name;
            public AnimationClip clip;
        }
        [Serializable]
        public class SwordAttackAnimationClips
        {
            [HideInInspector] public string name;
            public AnimationClip clip;
        }
        [Serializable]
        public class StickAttackAnimationClips
        {
            [HideInInspector] public string name;
            public AnimationClip clip;
        }
        [Serializable]
        public class BowAttackAnimationClips
        {
            [HideInInspector] public string name;
            public AnimationClip clip;
        }
        [Serializable]
        public class SlingshotAttackAnimationClips
        {
            [HideInInspector] public string name;
            public AnimationClip clip;
        }
        [Serializable]
        public class DeadAnimationClips
        {
            [HideInInspector] public string name;
            public AnimationClip clip;
        }

        public MoveAnimationClips[] MoveAnimClips;
        public IdleAnimationClips[] IdleAnimClips;
        public WithoutWeaponAttackAnimationClips[] WithoutWeaponAttackAnimClips;
        public SwordAttackAnimationClips[] SwordAttackAnimClips;
        public StickAttackAnimationClips[] StickAttackAnimClips;
        public BowAttackAnimationClips[] BowAttackAnimClips;
        public SlingshotAttackAnimationClips[] SlingshotAttackAnimClips;
        public DeadAnimationClips[] DeadAnimClips;

        public void SetNames() // “ехнический метод на релизе удалить
        {
            for(int i = 0; i < 4; i++) 
            {
                MoveAnimClips[i].name = $"{ (PlayerAnimationController.PlayerDirection)i}";
                IdleAnimClips[i].name = $"{ (PlayerAnimationController.PlayerDirection)i}";
                SwordAttackAnimClips[i].name = $"{ (PlayerAnimationController.PlayerDirection)i}";
                BowAttackAnimClips[i].name = $"{ (PlayerAnimationController.PlayerDirection)i}";
                SlingshotAttackAnimClips[i].name = $"{ (PlayerAnimationController.PlayerDirection)i}";
            }
        }

    }
}
