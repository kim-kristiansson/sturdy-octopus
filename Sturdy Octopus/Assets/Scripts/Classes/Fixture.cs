using System.Collections.Generic;
using UnityEngine;

public class Fixture : ProcessItem
{
    public List<StickAttachment> stickAttachments = new();
    public List<StickAttachment> initialStickAttachments = new();
    public GameObject stickPrefab;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    public Transform centralSpawnPoint;

    void Start()
    {
        foreach (StickAttachment stickAttachment in initialStickAttachments)
        {
            stickAttachment.SpawnStickAtPoint(stickPrefab);
        }
    }
}
