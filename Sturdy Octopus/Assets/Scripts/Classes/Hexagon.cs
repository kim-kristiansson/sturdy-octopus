using UnityEngine;

public class Hexagon : MonoBehaviour
{
    [SerializeField]
    private float _sideLength;

    public float SideLength { get { return _sideLength; } }

    void Start()
    {
        UpdateSideLength();
    }

    private void UpdateSideLength()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            float longDiameter = Mathf.Max(meshRenderer.bounds.size.x, meshRenderer.bounds.size.y); // Assuming hexagon lies flat on X-Y plane
            _sideLength = longDiameter / 2; // Calculating side length
            Debug.Log("Side Length: " + _sideLength);
        }
        else
        {
            Debug.LogError("MeshRenderer component not found. Disabling the hexagon.");
            this.enabled = false;
        }
    }
}