using Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] private ExperienceFactory _expFactory;
    [SerializeField] private List<DropChanceItem> _itemPrefabs = new();

    public List<Item> _itemsOnMap = new();

    public event UnityAction<Item> Spawned;

    public void Restart()
    {
        for (int i = _itemsOnMap.Count - 1; i >= 0; i--)
        {
            Destroy(_itemsOnMap[i].gameObject);
        }
        _itemPrefabs.Clear();
    }

    public bool TrySpawnRandom(Vector3 position, out Item item)
    {
        foreach (var drop in _itemPrefabs)
        {
            if(GetRandomValue() <= drop.Chance)
            {
                item = Instantiate(drop.Item, position, Quaternion.identity);
                item.Init(_expFactory);

                _itemsOnMap.Add(item);
                var cachedItem = item;
                item.Picked += () => _itemsOnMap.Remove(cachedItem);

                Spawned?.Invoke(item);
                return true;
            }
        }

        item = null;
        return false;

        float GetRandomValue() => Random.Range(0f, 100f);
    }
}
