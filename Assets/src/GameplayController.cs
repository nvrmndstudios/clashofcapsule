using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private BarrelSpawner _barrelSpawner;
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private UIController _uiController;

    private void Start()
    {
        _enemySpawner.OnWaveStarted += OnWaveStarted;
    }

    private void OnDestroy()
    {
        _enemySpawner.OnWaveStarted -= OnWaveStarted;
    }

    private void OnWaveStarted(int wave)
    {
        _uiController.UpdateWave(wave);
    }

    public void StartGame()
    {
        _enemySpawner.StartGame();
        _playerSpawner.SpawnPlayer();
        _barrelSpawner.StartGame();
    }

    public void EndGame()
    {
        _barrelSpawner.EndGame();
        _enemySpawner.EndGame();
    }  
}
