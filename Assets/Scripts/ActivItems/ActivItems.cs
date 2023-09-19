using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace MysteryForest
{
    public class ActivItems : MonoBehaviour
    {
        [SerializeField] private float _speedDeceleration = 1;
        //[SerializeField] private float _speedNavMesh = 1; TODO Убрать неиспользуемые поля

        [Inject] private UICanvasManager _uICanvasManager;

        public static bool Invisibility;
        public static bool ActiveItems = true;

        //private List<ActivItem> activList = new List<ActivItem>();
        private CircleCollider2D _circleCollider;
        private int _currentIndexActivItem;
        private void Start()
        {        
            _circleCollider = GetComponent<CircleCollider2D>();
            _circleCollider.enabled = false;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.TryGetComponent(out ActivItemsPrefab activItemsPrefab);

            if (!activItemsPrefab)
                return;

            if (!_uICanvasManager.TryGetComponent<UIShowActiveItems>(out var uIShowActiveItems))
            {
                Debug.LogError("Not UIShowActiveItems");
            }

            uIShowActiveItems.InstantiateActiveItems(activItemsPrefab);
        }
        private void Update()
        {
            if (!_uICanvasManager.TryGetComponent<UIShowActiveItems>(out var uIShowActiveItems))
            {
                Debug.LogError("Not UIShowActiveItems");
            }

            if (Input.GetKeyUp(KeyCode.T) && uIShowActiveItems.ActivationActiveItems.Count != 0)
            {
                ActivationActiveItems activationActiveItems = uIShowActiveItems.ActivationActiveItems[0];

                uIShowActiveItems.ActivationActiveItems.RemoveAt(0);

                ActivationActiveItems(activationActiveItems.ActivItem);

                Destroy(activationActiveItems.gameObject);
            }
        }

        public void ActivationActiveItems(ActivItem activItems)
        {
            switch (activItems)
            {
                case ActivItem.Home:
                    Home(activItems);
                    _currentIndexActivItem = ((int)ActivItem.Home);
                    UpdateStateLevelActivItem();
                    break;

                case ActivItem.RubikCube:
                    _currentIndexActivItem = ((int)ActivItem.RubikCube);
                    UpdateStateLevelActivItem();
                    StartCoroutine(Rubik());
                    break;

                case ActivItem.Firecracker:
                    _currentIndexActivItem = ((int)ActivItem.Firecracker);
                    UpdateStateLevelActivItem();
                    StartCoroutine(Firecracker());
                    break;

                case ActivItem.BucketOfFuelOil:
                    _currentIndexActivItem = ((int)ActivItem.BucketOfFuelOil);
                    UpdateStateLevelActivItem();
                    StartCoroutine(BucketOfFuelOil());
                    break;

                case ActivItem.BallWithPrediction:
                    _currentIndexActivItem = ((int)ActivItem.BallWithPrediction);
                    UpdateStateLevelActivItem();
                    StartCoroutine(BallWithPreditcion());
                    break;
            }
        }

        private void UpdateStateLevelActivItem()
        {
            if (_uICanvasManager.TryGetComponent<StatesLevel>(out StatesLevel statesLevel))
                statesLevel.UpdateImageActiveItem(_currentIndexActivItem);
        }

        private IEnumerator BallWithPreditcion()
        {
            if (_uICanvasManager.TryGetComponent(out FortuneBall fortuneBall))
                fortuneBall.TextShow();
            ActiveItems = false;

            yield return new WaitForSeconds(5f);

            ActiveItems = true;
            if (fortuneBall != null)
                fortuneBall.TextClose();
        }

        private IEnumerator Rubik()
        {
            AIControllerBase activItemsInvisibility =
                FindObjectsOfType<AIControllerBase>()
                .OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToArray()[0];

            if (activItemsInvisibility.TryGetComponent(out Fighter fighter))
            {
                fighter.Rigidbody.velocity = Vector2.zero;
                fighter.enabled = false;
            }

            if (activItemsInvisibility.TryGetComponent(out NavMeshAgent navMeshAgent))
            {
                navMeshAgent.enabled = false;
            }

            activItemsInvisibility.Invisibility = true;

            yield return new WaitForSeconds(5);

            activItemsInvisibility.Invisibility = false;

            if (fighter != null)
            {
                fighter.enabled = true;
            }
            else if (navMeshAgent != null)
            {
                navMeshAgent.enabled = true;
            }
        }

        private void Home(ActivItem activItems)
        {
            //activList.Add(activItems);

            //if (_item)
            //{
            //    StartCoroutine(Timer());
            //}

            StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            //_item = false;

            Invisibility = true;

            yield return new WaitForSeconds(3f);
            //activList.RemoveAt(0);

            Invisibility = false;

            //if (activList.Count > 0)
            //{
                //yield return new WaitForSeconds(12f);
                //StartCoroutine(Timer());
            //}
            //_item = true;
        }      
        
         private IEnumerator Firecracker()
         {
            AIControllerBase activItemsInvisibility = FindObjectsOfType<AIControllerBase>().OrderBy(x => Vector3.Distance(x.transform.position,
                                                                                                transform.position)).ToArray()[0];
            activItemsInvisibility.Invisibility = true;

            Rigidbody2D rigidbody2 = activItemsInvisibility.GetComponent<Rigidbody2D>();

            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(0.6f);
                rigidbody2.velocity = new Vector2(Random.Range(-4, 4), Random.Range(-4, 4));
            }
            activItemsInvisibility.Invisibility = false;
         }

        private IEnumerator BucketOfFuelOil()
        {
            _circleCollider.enabled = true;

            yield return new WaitForSeconds(5);

            _circleCollider.enabled = false;
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Fighter fighter))
            {
                fighter._moveSpeed = _speedDeceleration;
                fighter._maxMoveSpeed = _speedDeceleration - 1;
            }
            else if (collision.TryGetComponent(out AIHedgehog hedgeho))
            {
                hedgeho.Agent.speed = _speedDeceleration;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Fighter fighter))
            {
                fighter._moveSpeed = fighter._maxMoveSpeed;
                fighter._maxMoveSpeed = fighter._maxMoveSpeed - 1;
            }
            else if (collision.TryGetComponent(out AIHedgehog hedgehog))
            {
                hedgehog.Agent.speed = hedgehog.NavMeshSpeed;
            }
        }
    }
}