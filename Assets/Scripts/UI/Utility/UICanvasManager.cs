using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MysteryForest
{
    public enum CanvasType
    {
        MainMenu,
        GameUI,
        Prologue,
        Pause,
        Settings,
        WinScreen,
        LoseScreen,
        Authors,
        FinalPrologue
    }

    public class UICanvasManager : MonoBehaviour
    {
        [SerializeField] private GameObject _prologueCanvas;

        public List<UICanvasController> _canvasControllerList;
        public UICanvasController _lastActiveCanvas;

        public GameObject PrologueCanvas => _prologueCanvas;

        private void Awake()
        {
            _canvasControllerList.ForEach(controller => controller.gameObject.SetActive(false));
            SwitchCanvas(CanvasType.MainMenu);
        }

        public void SwitchCanvas(CanvasType _type)
        {
            if (_lastActiveCanvas != null)
            {
                _lastActiveCanvas.gameObject.SetActive(false);
            }

            UICanvasController desiredCanvas = _canvasControllerList.Find(x => x.canvasType == _type);
            if (desiredCanvas != null)
            {
                desiredCanvas.gameObject.SetActive(true);
                _lastActiveCanvas = desiredCanvas;
            }
            else { Debug.LogWarning("The desired canvas was not found!"); }
        }
    }
}
