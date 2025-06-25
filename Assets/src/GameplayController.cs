using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private BarrelSpawner _barrelSpawner;
    [SerializeField] private PlayerSpawner _playerSpawner;

    private void Start()
    {
        _enemySpawner.OnWaveStarted += OnWaveStarted;
    }

    private void OnWaveStarted(int wave)
    {
        Debug.Log("wave started " + wave);
    }

    public void StartGame()
    {
        _enemySpawner.IsGamePlaying = true;
        _playerSpawner.SpawnPlayer();
        _barrelSpawner.StartGame();
    }

    public void EndGame()
    {
        _enemySpawner.IsGamePlaying = false;
        _barrelSpawner.EndGame();
    }
}
