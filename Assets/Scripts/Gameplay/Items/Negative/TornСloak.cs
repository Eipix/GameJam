using DG.Tweening;
using Gameplay;
using System;

public class TornCloak : Item
{
    private const float MinSpeed = 1f;

    private readonly float _slowdownMultiplier = 0.5f;
    private readonly float _slowdownDuration = 5f;

    protected override void OnPlayerEnter(Player player)
    {
        if (player.TryGetComponent(out Health health) is false)
            throw new InvalidOperationException($"Игрок должен содержать компонент {nameof(Health)}");

        OnPicked();

        float speedDescreasing = player.DefaultSpeed * _slowdownMultiplier;

        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            player.ChangeSpeed(-speedDescreasing);
        });

        sequence.AppendInterval(_slowdownDuration);

        sequence.AppendCallback(() =>
        {
            player.ChangeSpeed(speedDescreasing);
        });

        Destroy(gameObject);
    }
}
