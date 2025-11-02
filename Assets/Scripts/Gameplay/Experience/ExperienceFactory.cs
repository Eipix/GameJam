using Common;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceFactory : Singleton<ExperienceFactory>
{
    [SerializeField] private UIProgressBar _progressBar;
    [SerializeField] private ExperienceParticle _particlePrefab;
    [SerializeField, Range(1, 10)] private int _particleSpawnedCount = 5;

    private List<ExperienceParticle> _activeParticles = new();

    public IReadOnlyList<ExperienceParticle> ActiveParticles => _activeParticles;

    public IReadOnlyList<ExperienceParticle> Spawn(Transform transform, int score)
    {
        List<ExperienceParticle> particles = new();

        int baseValue = score / _particleSpawnedCount;     // Основное значение (целая часть)
        int remainder = score % _particleSpawnedCount;     // Остаток

        SpawnParticle(baseValue + remainder, transform);

        for (int i = 1; i < _particleSpawnedCount; i++)
        {
            SpawnParticle(baseValue, transform);
        }

        return particles;
    }

    private ExperienceParticle SpawnParticle(int scorePerParticle, Transform transform)
    {
        var particle = Instantiate(_particlePrefab, transform.position, Quaternion.identity);
        particle.Init(transform, _progressBar, scorePerParticle);
        _activeParticles.Add(particle);
        particle.Collected += () => _activeParticles.Remove(particle);
        return particle;
    }
}
