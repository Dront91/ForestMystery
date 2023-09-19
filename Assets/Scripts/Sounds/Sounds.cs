using UnityEngine;
using UnityEngine.Audio;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace MysteryForest
{
    public enum MixerGroup
    {
        BGM,
        SFX,
        UISounds
    }
    [CreateAssetMenu()]
    public class Sounds : ScriptableObject
    {
        [Serializable]
        public class SoundTest
        {
            [HideInInspector] public string _soundName;
            public AudioClip _clip;
            public AudioMixerGroup _mixerGroup;
        }
        [SerializeField] private SoundTest[] m_Sounds;
        public SoundTest this[Sound s] => m_Sounds[(int)s];

#if UNITY_EDITOR
        [CustomEditor(typeof(Sounds))]
        public class SoundsInspector : Editor
        {
            private static readonly int soundCount = Enum.GetValues(typeof(Sound)).Length;
            private new Sounds target => base.target as Sounds;
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                if (target.m_Sounds.Length != soundCount)
                {
                    Array.Resize(ref target.m_Sounds, soundCount);
                }

                for (int i = 0; i < target.m_Sounds.Length; i++)
                {
                    string label = $"{(Sound)i}";
                    target.m_Sounds[i]._soundName = label;
                    
                }
            }
        }
#endif
    }
}
