using UnityEngine;
using System.Collections.Generic;

public class StickSpawner : MonoBehaviour
{
    public GameObject stickPrefab;
    public List<Transform> spawnPoints = new List<Transform>();
    public List<int> initialSpawnIndices = new List<int>();

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

    // ... Rest of your script ...
}
