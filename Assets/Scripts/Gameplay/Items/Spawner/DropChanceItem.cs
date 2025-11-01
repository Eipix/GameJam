using System;
using UnityEngine;

[Serializable]
public struct DropChanceItem
{
    [SerializeField] private Item _item;
    [SerializeField, Range(0, 100f)] private float _chance;

    public Item Item => _item;
    public float Chance => _chance;
}
