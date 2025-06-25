using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public PlayerSpawner playerSpawner;

    [Header("Wave Settings")]
    public float timeBetweenWaves = 3f;
    public float spawnInterval = 0.5f;

    public event Action<int> OnWaveStarted;

    private int currentWave = 0;
    private int enemiesRemaining = 0;
    private bool spawningWave = false;

    private List<GameObject> aliveEnemies = new List<GameObject>();
    private bool isGamePlaying = false;

    public void StartGame()
    {
        Debug.Log("EnemySpawner: Game Started");
        StopAllCoroutines(); // Stop any leftover coroutines
        ClearAllEnemies();

        currentWave = 0;
        enemiesRemaining = 0;
        isGamePlaying = true;
        spawningWave = false;
    }

    public void EndGame()
    {
        Debug.Log("EnemySpawner: Game Ended");
        StopAllCoroutines();
        ClearAllEnemies();
        isGamePlaying = false;
        spawningWave = false;
    }

    private void Update()
    {
        if (isGamePlaying)
        {
            Debug.Log($"Spawning: {spawningWave}, Enemies Left: {enemiesRemaining}");

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

        OnWaveStarted?.Invoke(currentWave);

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
        if (!isGamePlaying || enemyPrefab == null || spawnPoints.Length == 0) return;

        var player = playerSpawner.GetPlayer();
        if (player == null) return;

        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.player = player;
            enemyScript.OnEnemyDied += HandleEnemyDeath;
        }

        aliveEnemies.Add(enemy);
        enemiesRemaining++;
    }

    private void HandleEnemyDeath(GameObject enemy, bool canCountKill)
    {
        Debug.Log("EnemySpawner: Enemy Died");
        if(canCountKill)
            GameManager.Instance.OnEnemyKill(enemy);

        enemiesRemaining = Mathf.Max(0, enemiesRemaining - 1);
        aliveEnemies.Remove(enemy);
    }

    private void ClearAllEnemies()
    {
        foreach (var enemy in aliveEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        aliveEnemies.Clear();
        enemiesRemaining = 0;
    }

    private int CalculateEnemiesForWave(int wave)
    {
        return Mathf.Clamp(2 + (int)Mathf.Log(wave + 1, 2) * 3, 2, 100);
    }
}