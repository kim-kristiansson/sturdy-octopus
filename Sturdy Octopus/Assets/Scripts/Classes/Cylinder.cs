using System;
using UnityEngine;

public class Cylinder : ProcessItem
{
    private MeshRenderer meshRenderer;

    public float Length { get { return meshRenderer.bounds.size.y; } }
    public float Diameter { get { return MathF.Max(meshRenderer.bounds.size.x, meshRenderer.bounds.size.z); } }
    public float BaseaArea { get { return CalculateBaseArea(); } }

    void Awake()
    {
        if (!TryGetComponent<MeshRenderer>(out meshRenderer))
        {
            Debug.LogError("MeshRenderer component not found on the GameObject.", this);
            this.enabled = false;
        }
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

    private float CalculateBaseArea()
    {
        if (meshRenderer == null)
        {
            Debug.LogWarning("MeshRenderer is not assigned. Base area calculation might be incorrect.");
            return -1f;
        }

        float diameter = MathF.Max(meshRenderer.bounds.size.x, meshRenderer.bounds.size.z);
        float radius = diameter / 2f;

        return Mathf.PI * radius * radius;
    }
}