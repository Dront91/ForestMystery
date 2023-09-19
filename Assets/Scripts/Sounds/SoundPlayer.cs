using System;
using UnityEngine;
using UnityEngine.Audio;
namespace MysteryForest
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private Sounds _soundsList;
        public Sounds SoundList => _soundsList;
        [SerializeField] private AudioMixerGroup _backGroundMusicMixerGroup;
        [SerializeField] private BackGroundMusicController _backGroundMusicController;
        private AudioSource _audioSource;
        private void Start ()
        {
            _audioSource = GetComponent<AudioSource>();
            
        }
        public void Play(Sound sound)
        {
            if (_soundsList[sound] == null) return;
            if(_soundsList[sound]._clip == null || _soundsList[sound]._mixerGroup == null) 
            {
                Debug.LogError("Sound or mixer group not set, check Sounds!");
                return;
            }
            var clip = _soundsList[sound]._clip;
            _audioSource.outputAudioMixerGroup = _soundsList[sound]._mixerGroup;
            _audioSource.PlayOneShot(clip);
        }
    }
}