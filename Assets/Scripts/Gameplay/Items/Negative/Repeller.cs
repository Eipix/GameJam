using DG.Tweening;
using Gameplay;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Repeller : Item
{
    [SerializeField, Range(1, 100)] private float _force = 10f;

    protected override void OnPlayerEnter(Player player)
    {
        var activeParticles = ExperienceFactory.Instance.ActiveParticles.ToList();

        RepelFromPoint(activeParticles, player.transform.position);

        OnPicked();
        Destroy(gameObject);
    }
    public void RepelFromPoint(IEnumerable<ExperienceParticle> particles, Vector3 center)
    {
        foreach (var particle in particles)
        {
            Vector3 direction = particle.transform.position - center;
            direction.y = 0f;

            if (direction.magnitude > 0.01f)
            {
                direction.Normalize();
                Vector3 targetPosition = particle.transform.position + direction * _force;
                particle.transform.DOMove(targetPosition, 0.1f)
                             .SetEase(Ease.OutQuad);
            }
        }
    }
}
