using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    private class EnemyWave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public float spawnInterval;
        [HideInInspector] public int waveQuota;
        [HideInInspector] public int spawnCount;
    }

    [System.Serializable]
    private class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        [HideInInspector] public int enemySpawned;
        public GameObject enemyPrefab;
    }

    [SerializeField] private List<EnemyWave> _waves;
    [SerializeField] private List<Transform> enemySpawnPoints;
    [SerializeField] private float _waveInterval;
    [SerializeField] private float _maxEnemiesAllowed;
    [SerializeField] private float _enemySpawnRadiusX;
    [SerializeField] private float _enemySpawnRadiusY;
    private int enemiesAlive = 0;
    private bool maxEnemiesReached = false;
    private int currentWaveNumber;
    private Transform playerTransform;
    private float spawnTimer;

    void Start()
    {
        CalculateWaveQuota();
        playerTransform = FindFirstObjectByType<PlayerStats>().GetComponent<Transform>();
    }

    void Update()
    {
        if (currentWaveNumber < _waves.Count && _waves[currentWaveNumber].spawnCount == 0)
        {
            StartCoroutine(StartNextWave());
        }
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= _waves[currentWaveNumber].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(_waveInterval);
        if (currentWaveNumber < _waves.Count - 1)
        {
            currentWaveNumber++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        var currentWaveQuota = 0;
        foreach (var enemyGroup in _waves[currentWaveNumber].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        _waves[currentWaveNumber].waveQuota = currentWaveQuota;
    }

    void SpawnEnemies()
    {
        if (_waves[currentWaveNumber].spawnCount < _waves[currentWaveNumber].waveQuota && !maxEnemiesReached)
        {
            foreach (var enemyGroup in _waves[currentWaveNumber].enemyGroups)
            {
                if (enemyGroup.enemySpawned < enemyGroup.enemyCount)
                {
                    if (enemiesAlive >= _maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                    var enemy = Instantiate(enemyGroup.enemyPrefab, GenerateRandomSpawnPosition(), Quaternion.identity);
                    enemy.transform.SetParent(transform);
                    enemyGroup.enemySpawned++;
                    _waves[currentWaveNumber].spawnCount++;
                    enemiesAlive++;
                }
            }
        }
        if (enemiesAlive < _maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }
    public Vector3 GenerateRandomSpawnPosition()
    {
        var randomAngle = Random.value * 2 * Mathf.PI;
        var randomDirection = new Vector3(Mathf.Cos(randomAngle) * _enemySpawnRadiusX,
                                          Mathf.Sin(randomAngle) * _enemySpawnRadiusY, 0f);
        var spawnPosition = randomDirection + playerTransform.position;
        return spawnPosition;
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }
}

