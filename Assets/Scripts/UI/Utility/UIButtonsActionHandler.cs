using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MysteryForest
{
    public enum ButtonType
    {
        START_LEVEL,
        PAUSE,
        UNPAUSE,
        RESTART_LEVEL,
        MAIN_MENU,
        SETTINGS,
        CLOSESETTINGS,
        SETTINGSPAUSE,
        CLOSESETTINGSPAUSE,
        EXIT,
        NEXT_LEVEL,
        PLAY_PROLOGUE,
        AUTHORS,
        PLAY_FINISHPROLOGUE,
        WIN_PANEL
    }

    public class UIButtonsActionHandler : MonoBehaviour
    {
        [SerializeField] private ButtonType _buttonType;

        [Inject] private LevelSequenceController _levelSequenceController;
        [Inject] private UICanvasManager _canvasManager;
        [Inject] private SoundPlayer _soundPlayer; 
        
        private Button _menuButton;

        private void Start()
        {
            Initialize();
        }

        private void OnButtonClicked()
        {
            if (Time.timeScale == 0)
                Time.timeScale = 1;
            _soundPlayer.Play(Sound.MenuButtonClicked);
            switch (_buttonType)
            {
                case ButtonType.PLAY_PROLOGUE:
                    PlayPrologue();

                    break;
                case ButtonType.START_LEVEL:
                    _canvasManager.PrologueCanvas.SetActive(false);
                    _levelSequenceController.LoadLevel(_levelSequenceController.Levels[0]);
                    SwitchCanvasType(CanvasType.GameUI);
                    break;
                case ButtonType.PAUSE:
                    Time.timeScale = 0;
                    SwitchCanvasType(CanvasType.Pause);
                    break;
                case ButtonType.UNPAUSE:
                    SwitchCanvasType(CanvasType.GameUI);
                    break;
                case ButtonType.NEXT_LEVEL:
                    StartNextLevel();
                    break;
                case ButtonType.RESTART_LEVEL:
                    RestartLevel();
                    break;
                case ButtonType.MAIN_MENU:
                    _levelSequenceController.LoadMainMenu();
                    SwitchCanvasType(CanvasType.MainMenu);
                    break;
                case ButtonType.SETTINGS:
                    SwitchCanvasType(CanvasType.Settings);
                    break;
                case ButtonType.CLOSESETTINGS:
                    SwitchCanvasType(CanvasType.MainMenu);
                    break;
                case ButtonType.CLOSESETTINGSPAUSE:
                    SwitchCanvasType(CanvasType.Pause);
                    break;
                case ButtonType.EXIT:
                    Exit();
                    break;
                case ButtonType.AUTHORS:
                    SwitchCanvasType(CanvasType.Authors);
                    break;
                case ButtonType.PLAY_FINISHPROLOGUE:
                    SwitchCanvasType(CanvasType.FinalPrologue);
                    break;
                case ButtonType.WIN_PANEL:
                    SwitchCanvasType(CanvasType.WinScreen);
                    break;
                default:
                    break;
            }
        }

        private void PlayPrologue()
        {
            if(_canvasManager.PrologueCanvas != null)
            {
                _canvasManager.SwitchCanvas(CanvasType.Prologue);
                _levelSequenceController.InvokePlayPrologue();
            }
        }

        private void StartNextLevel()
        {
            _levelSequenceController.LoadNextLevel();
            _canvasManager.SwitchCanvas(CanvasType.GameUI);
        }

        private void RestartLevel()
        {
            _levelSequenceController.RestartLevel();
            _canvasManager.SwitchCanvas(CanvasType.GameUI);

        }

        private void SwitchCanvasType(CanvasType type)
        {
            _canvasManager.SwitchCanvas(type);
        }

        private static void Exit()
        {
            Application.Quit();
        }

        private void Initialize()
        {
            _menuButton = GetComponent<Button>();
            _menuButton.onClick.AddListener(OnButtonClicked);
        }
    }
}