using UnityEngine;

public class StickAttachment : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    public void SpawnStickAtPoint(GameObject stickPrefab)
    {
        Instantiate(stickPrefab, spawnPoint.position, Quaternion.identity);
    }
}
