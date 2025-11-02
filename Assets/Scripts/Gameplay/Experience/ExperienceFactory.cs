using Common;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceFactory : Singleton<ExperienceFactory>
{
    [SerializeField] private UIProgressBar _progressBar;
    [SerializeField, Range(1, 10)] private int _particleSpawnedCount = 5;
    [SerializeField] private ExperienceParticle _particlePrefab;

    public IReadOnlyList<ExperienceParticle> Spawn(Transform transform, int score)
    {
        List<ExperienceParticle> particles = new();

        int scorePerParticle = _particlePrefab.ScorePerParticle;

        int baseValue = score / _particleSpawnedCount;     // Основное значение (целая часть)
        int remainder = score % _particleSpawnedCount;     // Остаток

        particles.Add(SpawnParticle(baseValue + remainder, transform));

        for (int i = 1; i < _particleSpawnedCount; i++)
        {
            var particle = SpawnParticle(baseValue, transform);
            particles.Add(particle);
        }

        return particles;
    }

    private ExperienceParticle SpawnParticle(int scorePerParticle, Transform transform)
    {
        var particle = Instantiate(_particlePrefab, transform.position, Quaternion.identity);
        particle.Init(transform, _progressBar, scorePerParticle);
        return particle;
    }
}
