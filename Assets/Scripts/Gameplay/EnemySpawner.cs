using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class EnemySpawner : Singleton<EnemySpawner>
    {
        public delegate void WaveEndedHandler(int waveIndex);
        public event WaveEndedHandler WaveEnded;
        
        [System.Serializable]
        public class Wave
        {
            [field: SerializeField] public Cutscene Cutscene { get; private set; }
            [FormerlySerializedAs("enemyPrefab")] public List<Enemy> enemyPrefabs;
            public int maxEnemies;
            public Enemy bossPrefab;
        }

        [SerializeField] private List<Wave> waves = new List<Wave>();
        [SerializeField] private int maxActiveEnemies = 15;
        [SerializeField] private float spawnDistanceMin = 10f;
        [SerializeField] private float spawnDistanceMax = 20f;
        [SerializeField] private int maxSpawnAttempts = 10;
        [SerializeField] private Transform player;

        private int currentWaveIndex = 0;
        private int enemiesSpawnedInWave = 0;
        private List<Enemy> activeEnemies = new List<Enemy>();

        private void Start()
        {
            var wave = waves[currentWaveIndex];

            if(wave.Cutscene is null)
            {
                SpawnInitialEnemies();
                return;
            }

            wave.Cutscene.Launch();
            wave.Cutscene.Ended += SpawnInitialEnemies;
        }

        private void SpawnInitialEnemies()
        {
            for (int i = 0; i < maxActiveEnemies; i++)
            {
                TrySpawnEnemy();
            }
        }

        public void OnEnemyDeath(Enemy enemy)
        {
            if (activeEnemies.Contains(enemy))
            {
                activeEnemies.Remove(enemy);
                TrySpawnEnemy();
            }
        }

        private void TrySpawnEnemy()
        {
            if (currentWaveIndex >= waves.Count)
                return;

            Wave currentWave = waves[currentWaveIndex];

            if (enemiesSpawnedInWave >= currentWave.maxEnemies)
            {
                if (activeEnemies.Count == 0)
                {
                    BossSpawn();
                }
                return;
            }

            if (FindValidSpawnPosition(out var spawnPosition))
            {
                var prefab = GetRandomEnemyFromWave(currentWave);
                Enemy enemy = Instantiate(prefab, spawnPosition, Quaternion.identity);
                enemy.Init(player);
                activeEnemies.Add(enemy);
                enemiesSpawnedInWave++;

                Health health = enemy.GetComponent<Health>();
                health.OnDie += () => OnEnemyDeath(enemy);

            }
        }
        
        private void BossSpawn()
        {
            Wave currentWave = waves[currentWaveIndex];

            if (FindValidSpawnPosition(out var spawnPosition))
            {
                Enemy boss = Instantiate(currentWave.bossPrefab, spawnPosition, Quaternion.identity);
                boss.Init(player);
                activeEnemies.Add(boss);

                Health health = boss.GetComponent<Health>();
                health.OnDie += () =>
                {
                    if (activeEnemies.Contains(boss))
                    {
                        activeEnemies.Remove(boss);
                    }
                    
                    WaveEnded?.Invoke(currentWaveIndex);
                    NextWave();
                };
            }
            
        }
        
        private bool FindValidSpawnPosition(out Vector3 position)
        {
            position = Vector3.zero;

            for (int i = 0; i < maxSpawnAttempts; i++)
            {
                float angle = Random.Range(0f, 360f);
                float distance = Random.Range(spawnDistanceMin, spawnDistanceMax);

                Vector3 offset = new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad) * distance,
                    0,
                    Mathf.Sin(angle * Mathf.Deg2Rad) * distance
                );

                Vector3 testPosition = player.position + offset;

                if (NavMesh.SamplePosition(testPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    position = hit.position;
                    return true;
                }
            }

            return false;
        }

        private Enemy GetRandomEnemyFromWave(Wave wave)
        {
            int index = Random.Range(0, wave.enemyPrefabs.Count);
            return wave.enemyPrefabs[index];
        }
        
        private void NextWave()
        {
            currentWaveIndex++;
            enemiesSpawnedInWave = 0;

            if (currentWaveIndex < waves.Count)
            {
                SpawnInitialEnemies();
            }
        }
    }
}

