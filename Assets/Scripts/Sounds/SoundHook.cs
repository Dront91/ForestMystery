using UnityEngine;
using Zenject;
namespace MysteryForest
{
    public class SoundHook : MonoBehaviour
    {
        public Sound m_Sound;
        [Inject] protected SoundPlayer _soundPlayer;
        public void Play()
        {
            _soundPlayer.Play(m_Sound);
        }
    }
}
