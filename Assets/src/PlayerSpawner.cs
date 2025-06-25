using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameObject _playerPrefab;
    private PlayerController _currentPlayer;

    public PlayerController SpawnPlayer()
    {
        if (_currentPlayer == null)
        { 
            var playerObj = Instantiate(_playerPrefab, _spawnPosition.position, Quaternion.identity);
            _currentPlayer = playerObj.GetComponent<PlayerController>();
        }
        return _currentPlayer;
    }
}
