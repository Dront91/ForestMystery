using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MysteryForest
{
    public class UILearnText : MonoBehaviour
    {
       [SerializeField] private GameObject _canvasLearn;

        private bool _enter = false;
        private float _delay = 1.5f;

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (_canvasLearn != null)
            {
                if (!_enter)
                {
                    _canvasLearn.SetActive(false);
                    _enter = true;
                }
                else
                {
                    StartCoroutine(DelayCanvasOpen());
                }
            }
         
        }

        private IEnumerator DelayCanvasOpen()
        {
            yield return new WaitForSeconds(_delay);

            _canvasLearn.SetActive(true);
            _enter = false;
        }
    }
}

