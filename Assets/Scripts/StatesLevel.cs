using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MysteryForest
{

    public class StatesLevel : MonoBehaviour
    {
        [SerializeField] private Text _textKeys;
        [SerializeField] private Text[] _textBombs;
        [SerializeField] private GameObject[] _imagesWeapon;
        [SerializeField] private GameObject[] _imagesBombs;
        [SerializeField] private GameObject[] _imagesActivItems;
        [SerializeField] private GameObject _panelFortuneBall;
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _tutorialText;

        [Inject] private LevelSequenceController _levelSequenceController;

        // по деффолту пред индекс указывает на Without - гг без оружия
        private int _prevIndexWeapon = 0;

        public int PrevIndexWeapon => _prevIndexWeapon;

        private int _prevIndexBomb = 0;
        private int _prevIndexActivItem = 0;

        private void Start()
        {
            Initiliaze();
            _levelSequenceController.OnRestart += Initiliaze;

        }

        private void OnDestroy()
        {
            _levelSequenceController.OnRestart -= Initiliaze;
        }

        public void FadeOutTutorialText()
        {
            for (int i = 0; i < _tutorialText.transform.childCount; i++)
            {
                _tutorialText.transform.GetChild(i).GetComponent<Animator>().enabled = true;
            }

            StartCoroutine(CloseText());
        }

        private IEnumerator CloseText()
        {
            yield return new WaitForSeconds(2);

            _tutorialText.SetActive(false);
        }
        
        public void ShowTutorialText()
        {
            _tutorialText.SetActive(true);
        }

        public void UpdateWeapons(int _index)
        {
            _imagesWeapon[_prevIndexWeapon].SetActive(false);
            _imagesWeapon[_index].SetActive(true);
            _prevIndexWeapon = _index;
           
        }

        public void UpdateBombs(int _index)
        {
            _imagesBombs[_prevIndexBomb].SetActive(false);
            _textBombs[_prevIndexBomb].gameObject.SetActive(false);

            _imagesBombs[_index].SetActive(true);
            _textBombs[_index].gameObject.SetActive(true);

            _prevIndexBomb = _index;
        }

        public void UpdateTextKeys(int _countKeys)
        {
            _textKeys.text = _countKeys.ToString();
        }

        public void UpdateTextBombs(int _countBombs, int _indexType)
        {
            for (int i = 0; i < _textBombs.Length; i++)
            {
                if (i == _indexType)
                {
                    _textBombs[i].text = _countBombs.ToString();
                } 
            }
        }

        public void UpdateImageActiveItem(int _indexType)
        {
            _imagesActivItems[_prevIndexActivItem].SetActive(false);
            _imagesActivItems[_indexType].SetActive(true);

            _prevIndexActivItem = _indexType;
        }

        public void Initiliaze()
        {
            for (int i = 0; i < _textBombs.Length; i++)
            {
               _textBombs[i].text = "0";
            }

            for (int i = 0; i < _imagesActivItems.Length; i++)
            {
                _imagesActivItems[i].SetActive(false);
            }

            _textKeys.text = "0";

            if (_panelFortuneBall != null)
                _panelFortuneBall.SetActive(false);

            if (_content != null)
            {
                for (int i = 0; i < _content.transform.childCount; i++)
                {
                    Destroy(_content.transform.GetChild(i).gameObject);
                }
            }
        }
    }
}

