using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ExperienceFactory : MonoBehaviour
{
    [SerializeField] private UIProgressBar _progressBar;
    [SerializeField] private ExperienceParticle _particlePrefab;
    [SerializeField, Range(1, 10)] private int _maxParticleSpawnedCount = 5;

    private List<ExperienceParticle> _activeParticles = new();
    
    public IReadOnlyList<ExperienceParticle> ActiveParticles => _activeParticles;
    
    public int TotalExperienceOnMap => _activeParticles.Select(particle => particle.Score).Sum();

    public IReadOnlyList<ExperienceParticle>? Spawn(Transform transform, int score)
    {
        if (score <= 0)
            return null;

        List<ExperienceParticle> particles = new();
        
        int particleCount = score < _maxParticleSpawnedCount ? score : _maxParticleSpawnedCount;
        int baseValue = score / particleCount;
        int remainder = score % particleCount;

        SpawnParticle(baseValue + remainder, transform);

        for (int i = 1; i < particleCount; i++)
        {
            SpawnParticle(baseValue, transform);
        }
        
        Debug.LogWarning($"на карте {TotalExperienceOnMap} опыта");
        
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
