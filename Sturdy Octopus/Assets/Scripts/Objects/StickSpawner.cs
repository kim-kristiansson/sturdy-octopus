using UnityEngine;
using System.Collections.Generic;

public class StickSpawner : MonoBehaviour
{
    public GameObject stickPrefab;
    public List<Transform> spawnPoints = new();
    public List<int> initialSpawnIndices; // Indices of spawn points to spawn sticks at the start

    void Start()
    {
        // Spawn initial set of sticks
        foreach (int index in initialSpawnIndices)
        {
            SpawnStickAtPoint(index);
        }
    }

    public void SpawnStickAtPoint(int index)
    {
        if (index >= 0 && index < spawnPoints.Count)
        {
            Instantiate(stickPrefab, spawnPoints[index].position, Quaternion.identity);
        }
    }

    public void SpawnGroupOfSticks(List<int> indices)
    {
        foreach (int index in indices)
        {
            SpawnStickAtPoint(index);
        }
    }
}