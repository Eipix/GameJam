using Gameplay;
using System;

public class BloodBottle : Item
{
    private readonly float _multiplier = 1.1f;

    protected override void OnPlayerEnter(Player player)
    {
        if (player.gameObject.TryGetComponent(out Health health) is false)
            throw new InvalidOperationException($"Игрок должен содержать компонент {nameof(Health)}");

        OnPicked();
        health.Heal(health.CurrentHealth * _multiplier);
        Destroy(gameObject);
    }
}
