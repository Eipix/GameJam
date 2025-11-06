using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExperienceFactory : MonoBehaviour
{
    [SerializeField] private UIProgressBar _progressBar;
    [SerializeField] private ExperienceParticle _particlePrefab;
    [SerializeField, Range(1, 10)] private int _particleSpawnedCount = 5;

    private List<ExperienceParticle> _activeParticles = new();
    
    public IReadOnlyList<ExperienceParticle> ActiveParticles => _activeParticles;
    
    public int TotalExperienceOnMap => _activeParticles.Select(particle => particle.Score).Sum();

    public IReadOnlyList<ExperienceParticle>? Spawn(Transform transform, int score)
    {
        if (score <= 0)
            return null;

        List<ExperienceParticle> particles = new();

        int baseValue = score / _particleSpawnedCount;     // �������� �������� (����� �����)
        int remainder = score % _particleSpawnedCount;     // �������``````````

        SpawnParticle(baseValue + remainder, transform);

        for (int i = 1; i < _particleSpawnedCount; i++)
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
