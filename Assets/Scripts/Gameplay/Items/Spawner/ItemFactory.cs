using Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemFactory : Singleton<ItemFactory>
{
    [SerializeField] private List<DropChanceItem> _itemPrefabs = new();

    public event UnityAction<Item> Spawned;

    public bool TrySpawnRandom(Vector3 position, out Item item)
    {
        foreach (var drop in _itemPrefabs)
        {
            if(GetRandomValue() <= drop.Chance)
            {
                item = Instantiate(drop.Item, position, Quaternion.identity);
                Spawned?.Invoke(item);
                return true;
            }
        }

        item = null;
        return false;

        float GetRandomValue() => Random.Range(0f, 100f);
    }
}
