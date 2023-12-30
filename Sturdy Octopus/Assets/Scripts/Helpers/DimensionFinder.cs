using UnityEngine;

public class DimensionFinder : MonoBehaviour
{
    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            float height = meshRenderer.bounds.size.y; // Height of the object
            float width = meshRenderer.bounds.size.x;  // Width of the object
            float depth = meshRenderer.bounds.size.z;  // Depth of the object

            Debug.Log("Height: " + height + ", Width: " + width + ", Depth: " + depth);
        }
    }
}
