using Gameplay;
using System;
using UnityEngine;

public class BloodBottle : Item
{
    private readonly float _multiplier = 0.25f;

    protected override void OnPlayerEnter(Player player)
    {
        if (player.gameObject.TryGetComponent(out Health health) is false)
            throw new InvalidOperationException($"����� ������ ��������� ��������� {nameof(Health)}");

        OnPicked();
        health.Heal(health.CurrentHealth * _multiplier);

        Destroy(gameObject);
    }
}
