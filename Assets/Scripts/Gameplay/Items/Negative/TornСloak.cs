using DG.Tweening;
using Gameplay;
using System;

public class TornCloak : Item
{
    private readonly float _slowdownMultiplier = 0.5f;
    private readonly float _slowdownDuration = 5f;

    protected override void OnPlayerEnter(Player player)
    {
        if (player.TryGetComponent(out Health health) is false)
            throw new InvalidOperationException($"����� ������ ��������� ��������� {nameof(Health)}");

        float speedBonus = player.DefaultSpeed * _slowdownMultiplier;

        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            player.ChangeSpeed(-speedBonus);
        });

        sequence.AppendInterval(_slowdownDuration);

        sequence.AppendCallback(() =>
        {
            player.ChangeSpeed(speedBonus);
        });
    }
}
