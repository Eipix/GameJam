using Gameplay;
using System;

public class PosionBottle : Item
{
    private readonly float _multiplier = 0.1f;

    protected override void OnPlayerEnter(Player player)
    {
        if (player.gameObject.TryGetComponent(out Health health) is false)
            throw new InvalidOperationException($"Игрок должен содержать компонент {nameof(Health)}");

        OnPicked();

        float damage = health.CurrentHealth * _multiplier;

        health.TakeDamage(damage, gameObject);
        Destroy(gameObject);
    }
}
