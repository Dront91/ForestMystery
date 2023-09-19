using Zenject;
using UnityEngine;
namespace MysteryForest
{
    [RequireComponent(typeof(AudioSource))]
    public class BackGroundMusicController : MonoBehaviour
    {
        [SerializeField] private Sounds _soundsList;
        public Sound[] FriendlyTracks;
        public Sound[] BattleTracks;
        public Sound[] MainMenuTracks;
        public Sound[] StoryModeTracks;
        [Inject] private LevelSequenceController _levelSequenceController;
        private Status _currentStatus;
        private Sound _nextTrack;
        private AudioSource _audioSource;
        private enum Status
        {
            IsBattle,
            IsFriendly,
            IsMainMenu,
            IsStoryMode
        }
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _levelSequenceController.OnMainMenuLoaded += PlayMainMenuMusic;
            _levelSequenceController.OnPrologueStart += PlayPrologueMusic;
            _currentStatus = Status.IsMainMenu;
        }
        private void Update()
        {
            if(_audioSource.isPlaying == false) 
            {
                RestartBGM();
            }
        }
        private void OnDestroy()
        {
            _levelSequenceController.OnMainMenuLoaded -= PlayMainMenuMusic;
            _levelSequenceController.OnPrologueStart -= PlayPrologueMusic;
        }
        public void RestartBGM()
        {
            CheckStatusAndChoseNextTrack();
            PlayNewTrack();
        }
        private void CheckStatusAndChoseNextTrack()
        { 
            switch(_currentStatus)
            {
                case Status.IsBattle :  
                    _nextTrack = BattleTracks[Random.Range(0, BattleTracks.Length)]; 
                    break;
                case Status.IsFriendly :
                    _nextTrack = FriendlyTracks[Random.Range(0, FriendlyTracks.Length)];
                    break;
                case Status.IsMainMenu:
                    _nextTrack = MainMenuTracks[Random.Range(0, MainMenuTracks.Length)];
                    break;
                case Status.IsStoryMode:
                    _nextTrack = StoryModeTracks[Random.Range(0, StoryModeTracks.Length)];
                    break;
            }
        }
        public void PlayBattleMusic()
        {
            _currentStatus = Status.IsBattle;
            RestartBGM();
        }
        public void PlayFriendlyMusic()
        {
            if (_currentStatus == Status.IsFriendly) return;
            _currentStatus = Status.IsFriendly;
            RestartBGM();
        }
        private void PlayMainMenuMusic()
        {
            _currentStatus = Status.IsMainMenu;
            RestartBGM();
        }
        private void PlayPrologueMusic()
        {
            _currentStatus = Status.IsStoryMode;
            RestartBGM();
        }
        private void PlayNewTrack()
        {
            if (_soundsList[_nextTrack] == null) return;
            _audioSource.Stop();
            _audioSource.clip = _soundsList[_nextTrack]._clip;
            _audioSource.Play();
        }
    }
}
