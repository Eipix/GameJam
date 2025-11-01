using Common;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Singleton<ItemSpawner>
{
    [SerializeField] private List<DropChanceItem> _itemPrefabs = new();

    public bool TrySpawnRandom(Vector3 position, out Item item)
    {
        foreach (var drop in _itemPrefabs)
        {
            if(GetRandomValue() <= drop.Chance)
            {
                item = Instantiate(drop.Item, position, Quaternion.identity);
                return true;
            }
        }

        item = null;
        return false;

        float GetRandomValue() => Random.Range(0f, 100f);
    }
}
