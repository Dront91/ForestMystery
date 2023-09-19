using UnityEngine;
using UnityEngine.UI;

namespace MysteryForest
{
  
    public class FortuneBall : MonoBehaviour
    {
        [SerializeField] private ItemBallWithPredictions _textFortuneBall;
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _panel;

        private void Awake()
        {
            _panel.GetComponent<GameObject>();
            _text.GetComponent<Text>();
        }

        public void TextShow()
        {
            if (_panel != null && _text != null && ActivItems.ActiveItems)
            {
                TextClose();
                _panel.SetActive(true);
                int randIndex = Random.Range(0, _textFortuneBall.Text.Length);
                _text.text = _textFortuneBall.Text[randIndex].ToString();
            }
        }

        public void TextClose()
        {
            _panel.SetActive(false);
        }

    }
}