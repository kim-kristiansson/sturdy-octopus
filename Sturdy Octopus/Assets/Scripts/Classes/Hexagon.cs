using UnityEngine;

public class Hexagon : MonoBehaviour
{
    public float SideLength { get { return CalculateSideLength(); } }

    public float Area
    {
        get { return CalculateHexagonArea(); }
    }

    void Start()
    {
        CalculateSideLength();
    }

    private float CalculateSideLength()
    {

        if (TryGetComponent<MeshRenderer>(out var meshRenderer))
        {
            float longDiameter = Mathf.Max(meshRenderer.bounds.size.x, meshRenderer.bounds.size.y); // Assuming hexagon lies flat on X-Y plane
            float sideLength = longDiameter / 2; // Calculating side length
            Debug.Log("Side Length: " + sideLength);

            return sideLength;
        }
        else
        {
            Debug.LogError("MeshRenderer component not found. Disabling the hexagon.");
            this.enabled = false;

            return -1;
        }
    }

    private float CalculateHexagonArea()
    {
        float sideLength = CalculateSideLength();
        return 3 * Mathf.Sqrt(3) / 2 * sideLength * sideLength;
    }
}
