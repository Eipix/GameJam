using Gameplay;
using System.Linq;

public class Magnet : Item
{
    protected override void OnPlayerEnter(Player player)
    {
        var activeParticles = ExperienceFactory.Instance.ActiveParticles.ToList();

        foreach (var particle in activeParticles)
        {
            particle.ForceCollect(player);
        }

        OnPicked();
        Destroy(gameObject);
    }
}
