using Zenject;
using UnityEngine;
namespace MysteryForest
{
    public class HornetSoundController : BaseEnemySoundController
    {
        private float _timer;
        private HornetAI _hornetAI;
        private RoomController _roomController;
        [Inject] private SoundPlayer _soundPlayer;
        protected override void Start()
        {
            base.Start();
            _hornetAI = GetComponentInParent<HornetAI>();
            _roomController = GetComponentInParent<RoomController>();
            _fighter.DamageTaken += OnDamageTaken;
            if (_roomController != null && _roomController.IsActive == true)
            {
                _audioClip = Sound.HornetFly;
                PlaySound();
            }
        }
        private void Update()
        {
            _timer += Time.deltaTime;
            if(_timer >= _soundPlayer.SoundList[Sound.HornetFly]._clip.length && _hornetAI.NPCSightDistance != 0f)
            {
                _timer = 0;
                _audioClip = Sound.HornetFly;
                PlaySound();
            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _fighter.DamageTaken -= OnDamageTaken;
        }

        private void OnDamageTaken()
        {
            _audioClip = Sound.HornetDamageResive;
            PlaySound();
        }
    }
}
