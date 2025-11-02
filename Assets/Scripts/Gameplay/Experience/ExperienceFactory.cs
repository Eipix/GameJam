using System;
using UnityEngine;
using Common;

public class ExperienceFactory : Singleton<ExperienceFactory>
{
    [SerializeField] private ExperienceParticle _particlePrefab;

    public void Spawn(int score)
    {
        int scorePerParticle = _particlePrefab.ScorePerParticle;

        if (score < scorePerParticle)
            throw new InvalidOperationException($"Необходимые опыт ({score}) меньше чем очки за 1 частицу ({scorePerParticle}). Повысьте необходимые опыт или измените опыт который дает 1 частица в ее префабе.");
        
        if (score % scorePerParticle is 0)
        {
            float spawnCount = score / scorePerParticle;
            throw new InvalidOperationException($"Невозможно заспавнить {Conve}.Опыт за 1 частицу составляет {scorePerParticle}");
        }
    }
}
