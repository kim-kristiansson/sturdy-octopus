using System;
using Unity.VisualScripting;
using UnityEngine;

public class Cylinder : ProcessItem
{
    private MeshRenderer meshRenderer;
    [SerializeField] private float _length;
    [SerializeField] private float _diameter;

    public float Length { get { return meshRenderer.bounds.size.y; } }
    public float Diameter { get { return MathF.Max(meshRenderer.bounds.size.x, meshRenderer.bounds.size.z); } }
    public float Weight { get { return _weight; } }

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetPhysicalProperties(float length, float diameter, float weight)
    {
        if (length <= 0 || diameter <= 0 || weight <= 0)
        {
            Debug.LogError("Invalid dimensions or weight. Length, diameter, and weight must be positive.");
            return;
        }

        transform.localScale = new Vector3(diameter, length, diameter);
    }
}