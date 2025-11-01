using DG.Tweening;
using Gameplay;
using System;

public class DraculasCloak : Item
{
    private readonly float _speedMultiplier = 2f;
    private readonly float _invincibilityAndSpeedDuration = 5f;

    protected override void OnPlayerEnter(Player player)
    {
        if (player.TryGetComponent(out Health health) is false)
            throw new InvalidOperationException($"����� ������ ��������� ��������� {nameof(Health)}");

        OnPicked();

        float speedBonus = player.DefaultSpeed * _speedMultiplier;

        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            player.ChangeSpeed(speedBonus);
            health.Invincibility = true;
        });

        sequence.AppendInterval(_invincibilityAndSpeedDuration);

        sequence.AppendCallback(() =>
        {
            player.ChangeSpeed(-speedBonus);
            health.Invincibility = false;
        });
    }
}
