using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [Header("Barrel Settings")]
    public GameObject barrelPrefab;

    [Header("Spawn Range (XZ)")]
    public float minX = -5f;
    public float maxX = 5f;
    public float minZ = -5f;
    public float maxZ = 5f;
    public float spawnY = 0f; // Fixed Y height

    [Header("Spawn Logic")]
    public float respawnDelay = 0.3f; // Delay before new spawn (in seconds)

    private List<GameObject> activeBarrels = new List<GameObject>();

    public void StartGame()
    {
        StartCoroutine(SpawnBarrelsLoop());
    }

    public void EndGame()
    {
        StopCoroutine(SpawnBarrelsLoop());
    }

    private IEnumerator SpawnBarrelsLoop()
    {
        while (true)
        {
            yield return new WaitUntil(() => activeBarrels.Count == 0);
            yield return new WaitForSeconds(respawnDelay);

            int barrelCount = Random.Range(1, 3); // 1 or 2 barrels
            for (int i = 0; i < barrelCount; i++)
            {
                Vector3 randomPosition = GetRandomSpawnPosition();
                GameObject barrel = Instantiate(barrelPrefab, randomPosition, Quaternion.identity);
                activeBarrels.Add(barrel);

                Barrel barrelScript = barrel.GetComponent<Barrel>();
                if (barrelScript != null)
                {
                    barrelScript.OnBarrelBlasted += HandleBarrelBlasted;
                }
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);
        return new Vector3(x, spawnY, z);
    }

    private void HandleBarrelBlasted(GameObject barrel)
    {
        activeBarrels.Remove(barrel);
    }
}