using Gameplay;

public class Magnet : Item
{
    protected override void OnPlayerEnter(Player player)
    {
        var activeParticles = ExperienceFactory.Instance.ActiveParticles;

        foreach (var particle in activeParticles)
        {
            particle.ForceCollect(player);
        }

        OnPicked();
    }
}
