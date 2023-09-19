using UnityEngine;
using Zenject;
namespace MysteryForest
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] protected EntityAsset _entityAsset;
        [SerializeField] protected Entity _entityPrefab;
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        private DiContainer _dIContainer;
        public EntityAsset EntityAsset => _entityAsset;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        private float _dropChance;
        private int _itemValue;
        public int ItemValue => _itemValue;

        public virtual void Start()
        {
            ApplyPreferences(_entityAsset);
        }

        // ����� ��� ���� ����� ����� �������� ����� ������ ���� ���� ��������� �� ������� ����� ��������,
        // ��������� ���� � ��� �������� ��������� ������ � ������� � � ������� �������� ������ ����� �� �����������, �� ��� ����� ������� � �������������� ��� �������������.
        public float GetDropChanceFromChest()
        {
            if (_entityAsset is DropFromChestAsset)
            {
                switch (_entityAsset.Rarity)
                {
                    case Rarity.Common:
                        _dropChance = (_entityAsset as DropFromChestAsset).DropChanceFromCommonChest;
                        return _dropChance;
                    case Rarity.Uncommon:
                        _dropChance = (_entityAsset as DropFromChestAsset).DropChanceFromUmcommonChest;
                        return _dropChance;
                    case Rarity.Rare:
                        _dropChance = (_entityAsset as DropFromChestAsset).DropChanceFromRareChest;
                        return _dropChance;
                    case Rarity.Epic:
                        _dropChance = (_entityAsset as DropFromChestAsset).DropChanceFromEpicChest;
                        return _dropChance;
                    case Rarity.Legendary:
                        _dropChance = (_entityAsset as DropFromChestAsset).DropChanceFromLegendaryChest;
                        return _dropChance;

                    default:
                        Debug.Log("Drop chance not set, check it!");
                        return 0;
                }
            }
            else
            {
                Debug.Log("This item cannot drop from any chest!");
                return 0;
            }
        }
        public float GetDropChanceFromEnemy(EnemyInventory.Enemy enemy)
        {
            if (_entityAsset is DropFromEnemyAsset)
            {
                switch (enemy)
                {
                    case EnemyInventory.Enemy.Squirrel:
                        _dropChance = (_entityAsset as DropFromEnemyAsset).DropChanceFromSquirrel;
                        return _dropChance;
                    case EnemyInventory.Enemy.Mole:
                        _dropChance = (_entityAsset as DropFromEnemyAsset).DropChanceFromMole;
                        return _dropChance;
                    case EnemyInventory.Enemy.Hornet:
                        _dropChance = (_entityAsset as DropFromEnemyAsset).DropChanceFromHornet;
                        return _dropChance;
                    case EnemyInventory.Enemy.Hive:
                        _dropChance = (_entityAsset as DropFromEnemyAsset).DropChanceFromHive;
                        return _dropChance;
                    case EnemyInventory.Enemy.Hedgehog:
                        _dropChance = (_entityAsset as DropFromEnemyAsset).DropChanceFromHedgehog;
                        return _dropChance;
                    case EnemyInventory.Enemy.Rabbit:
                        _dropChance = (_entityAsset as DropFromEnemyAsset).DropChanceFromRabbit;
                        return _dropChance;
                    case EnemyInventory.Enemy.SnakeBoss:
                        _dropChance = (_entityAsset as DropFromEnemyAsset).DropChanceFromSnakeBoss;
                        return _dropChance;
                    default:
                        Debug.Log("Drop chance not set, check it!");
                        return 0;
                }
            }
            else
            {
                Debug.Log("This item cannot drop from any enemy!");
                return 0;
            }
        }
        // ����������� ����� ������� ����� ���������/������� ����� �� ����� Entity, ������� ����� ���� ����� �������� ������, ������ �� ��������� � ������������ ���������� � ������� ���������� ����� ������
        public virtual void SpawnEntity(Vector2 spawnPoint, DiContainer _diContainer, Transform parentTransform)
        {
            _dIContainer = _diContainer;
            Vector3 point = new Vector3(spawnPoint.x, spawnPoint.y, 0);
            var e = _diContainer.InstantiatePrefabForComponent<Entity>(_entityPrefab, point, Quaternion.identity, parentTransform);
            //var e = Instantiate(_entityPrefab, point, Quaternion.identity);
            e.ApplyPreferences(_entityAsset);

        }
        // ����������� ����� ��� ���������� �������������� �������� ������ Entity, ���������������� � ������ ���� Entity, � ������� �������� ������.
        public virtual void ApplyPreferences(EntityAsset asset)
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _spriteRenderer.sprite = asset.Sprite;
            if (asset is DropFromChestAsset)
            {
                _itemValue = (asset as DropFromChestAsset).ItemValue;
            }
            else
            {
                _itemValue = 0;
            }

        }
        public int GetItemValue()
        {

            if (_entityAsset is DropFromChestAsset)
            {
                _itemValue = (_entityAsset as DropFromChestAsset).ItemValue;
            }
            else
            {
                _itemValue = 0;
            }
            return _itemValue;
        }


    }
}

