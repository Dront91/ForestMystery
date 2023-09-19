using UnityEngine.SceneManagement;
using UnityEngine;
using Zenject;
namespace MysteryForest
{
    [RequireComponent(typeof(AudioSource))]
    public class AmbientLevelSounds : MonoBehaviour
    {
        [SerializeField] private Sounds _soundsList;
        private AudioSource _audioSource;
        [Inject] private LevelSequenceController _levelSequenceController;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            SceneManager.activeSceneChanged += OnSceneChange;
        }
        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
        private void Update()
        {
            if (_audioSource.isPlaying == false && _levelSequenceController.CurrentLevel != null)
            {
                RestartMusic();
            }
        }
        private void OnSceneChange(Scene previousScene, Scene newScene)
        {
            RestartMusic();
        }
        private void RestartMusic()
        {
            _audioSource.Stop();
            if (_levelSequenceController.CurrentLevel == null) return;
            _audioSource.clip = _soundsList[_levelSequenceController.CurrentLevel.AmbientSound]._clip;
            _audioSource.Play();
        }
    }
}

