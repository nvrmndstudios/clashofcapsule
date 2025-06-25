using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public Transform player;
    public Transform[] spawnPoints;

    [Header("Wave Settings")]
    public float timeBetweenWaves = 3f;
    public float spawnInterval = 0.5f;

    public event Action<int> OnWaveStarted; // Event for UI or other systems

    private int currentWave = 0;
    private int enemiesRemaining = 0;
    private bool spawningWave = false;

    private List<GameObject> aliveEnemies = new List<GameObject>();

    public bool IsGamePlaying = false;

    private void Update()
    {
        if (IsGamePlaying)
        {
            if (!spawningWave && enemiesRemaining == 0)
            {
                StartCoroutine(SpawnWave());
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        spawningWave = true;

        currentWave++;
        int enemyCount = CalculateEnemiesForWave(currentWave);

        OnWaveStarted?.Invoke(currentWave); // Fire event

        yield return new WaitForSeconds(timeBetweenWaves);

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }

        spawningWave = false;
    }

    private void SpawnEnemy()
    {
        if (!IsGamePlaying)
        {
            return;
        }

        if (enemyPrefab == null || player == null || spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.player = player.GetComponent<PlayerController>();
            enemyScript.OnEnemyDied += HandleEnemyDeath;
        }

        aliveEnemies.Add(enemy);
        enemiesRemaining++;
    }

    private void HandleEnemyDeath(GameObject enemy)
    {
        enemiesRemaining--;
        aliveEnemies.Remove(enemy);
    }

    private int CalculateEnemiesForWave(int wave)
    {
        return Mathf.Clamp(2 + (int)Mathf.Log(wave + 1, 2) * 3, 2, 100); // Dynamic logic
    }
}