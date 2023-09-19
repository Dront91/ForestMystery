using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MysteryForest
{
    public class PassiveItems : MonoBehaviour
    {
        private int _socks = 0;
        private Fighter _fighter;

        [Inject] private UICanvasManager _canvasManager;

        [SerializeField] private float _twoSocksCrete = 0.3f;
        [SerializeField] private float _socksCrete = 0.05f;
        [SerializeField] private float _midgeBonusSpeed = 0.1f;
        [SerializeField] private float _birdsBonusSpeed = 0.3f;
        [SerializeField] private float _chipsBonusHP = 0.02f;
        [SerializeField] private float _berryBonusHP = -0.3f;
        [SerializeField] private float _berryBonusAttack = 0.2f;
        [SerializeField] private float _gearWhealCrete = 0.15f;
        [SerializeField] private float _drinkCrete = 0.15f;
        [SerializeField] private float _headphonesCrete = 0.1f;

        [SerializeField] private ActivationPassiveItem _activationPassiveItemsPrefab;

        private List<ActivationPassiveItem> _activationActiveItems = new List<ActivationPassiveItem>();
        
        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.TryGetComponent(out TestPassiveItem passivItemsPrefab);      

            if (passivItemsPrefab == null)
                return;


            TestPassiveItem.passivItems passiv = passivItemsPrefab._passivItems;

            if (_canvasManager.TryGetComponent<UIShowPassiveItems>(out var uIShowPassive))
            {
                uIShowPassive.InstantiatePassivItem(passivItemsPrefab);
            }


            switch (passiv)
            {
                case TestPassiveItem.passivItems.Chips:
                    StartCoroutine(Enumerator());
                    break;

                case TestPassiveItem.passivItems.Berry:
                    Berry();
                    break;

                case TestPassiveItem.passivItems.Bracelet:
                    Bracelet();
                    break;

                case TestPassiveItem.passivItems.RedSock:
                    TwoSocks();
                    break;

                case TestPassiveItem.passivItems.BlueSock:
                    TwoSocks();
                    break;

                case TestPassiveItem.passivItems.Drink:
                    Drink();
                    break;

                case TestPassiveItem.passivItems.Midge:
                    BonusSpeed(passiv);
                    break;

                case TestPassiveItem.passivItems.Hummingbirds:
                    BonusSpeed(passiv);
                    break;

                case TestPassiveItem.passivItems.GearWheel:
                    GearWheel();
                    break;

                case TestPassiveItem.passivItems.Headphones:
                    Headphones();
                    break;
            }
        }
        private void TwoSocks()
        {
            if(_socks == 1)
            {
                _fighter.UpdateCreteChance(_twoSocksCrete);
            }
            else
            {
                _fighter.UpdateCreteChance(_socksCrete);
                //Socks++;
            }
        }
        private void BonusSpeed(TestPassiveItem.passivItems passivItems)
        {
            if (passivItems == TestPassiveItem.passivItems.Midge)
            {              
                _fighter._moveSpeed += (int)(_fighter._moveSpeed * _midgeBonusSpeed);
            }
            else
            {
                _fighter.DefoldSpeed();
                _fighter._moveSpeed += (int)(_fighter._moveSpeed * _birdsBonusSpeed);
            }
        }
        private IEnumerator Enumerator()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(1);
                _fighter.BonusHitPoint(_chipsBonusHP);
            }
        }
        private void Berry() 
        {
            _fighter.BonusHitPoint(_berryBonusHP);
            _fighter.BonusAttack(_berryBonusAttack);       
        }
        private void GearWheel()
        {
            _fighter.UpdateCreteChance(_gearWhealCrete);
        }
        private void Drink()
        {
            _fighter.UpdateCreteDamageCoef(_drinkCrete);
        }
        private void Bracelet()
        {          
            _fighter._protection = true;
        }
        private void Headphones()
        {
            _fighter.UpdateCreteDamageCoef(_headphonesCrete);
        }
    }
   
}