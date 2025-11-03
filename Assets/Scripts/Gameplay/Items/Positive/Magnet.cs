using Gameplay;
using System.Linq;

public class Magnet : Item
{
    protected override void OnPlayerEnter(Player player)
    {
        var activeParticles = ExperienceFactory.ActiveParticles.ToList();

        foreach (var particle in activeParticles)
        {
            if (particle.IsCollected)
                continue;

            particle.ForceCollect(player);
        }

        OnPicked();
        Destroy(gameObject);
    }
}
