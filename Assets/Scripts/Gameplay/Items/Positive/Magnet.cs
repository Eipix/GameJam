using Gameplay;
using System.Linq;
using UnityEngine;

public class Magnet : Item
{
    protected override void OnPlayerEnter(Player player)
    {
        var activeParticles = ExperienceFactory.ActiveParticles.ToList();
        
        int expCollected = 0;
        foreach (var particle in activeParticles)
        {
            if (particle.IsCollected)
                continue;

            particle.ForceCollect(player);
            expCollected += particle.Score;
        }
        
        Debug.LogWarning($"Собрано {expCollected} опыта");

        OnPicked();
        Destroy(gameObject);
    }
}
