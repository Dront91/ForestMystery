using UnityEngine;
using Zenject;

namespace MysteryForest
{
    public class LevelCompletion : MonoBehaviour
    {
        [SerializeField] private RoomController _lastRoom;
        [SerializeField] private SoundHook _winScreenSoundHook;
        [Inject] private LevelSequenceController _levelSequenceController;
        [Inject] private UICanvasManager _UICanvasManager;

        private void Start()
        {
            _lastRoom.OnRoomCompleted += OnLevelComleted;
        }
        private void OnDestroy()
        {
            _lastRoom.OnRoomCompleted -= OnLevelComleted;
        }

        private void OnLevelComleted()
        {
            _UICanvasManager.SwitchCanvas(CanvasType.WinScreen);
            _winScreenSoundHook.Play();
            _levelSequenceController.InvokeLevelComplete();

        }
    }
}
