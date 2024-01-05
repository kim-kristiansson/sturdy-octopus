using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class HexagonPacker : MonoBehaviour
{
    public float hexagonSideLength = 5f;
    public GameObject cylinderPrefab;
    private readonly List<Vector2> hexVertices = new List<Vector2>();
    public TMP_InputField diameterInputField;
    public Button generateButton;
    private List<Vector3> positions;

    private const float MinDiameter = 0.1f; // Set your minimum diameter
    private const float MaxDiameter = 10.0f; // Set your maximum diameter

    void Start()
    {
        generateButton.onClick.AddListener(GenerateCylinders);
    }

    private void GenerateCylinders()
    {
        if (float.TryParse(diameterInputField.text, out float diameter))
        {
            diameter = Mathf.Clamp(diameter, MinDiameter, MaxDiameter);

            // Calculate hexagon vertices based on the new diameter
            CalculateHexagonVertices(); // Ensure vertices are updated before generating cylinders

            SetCylinderDiameter(diameter);
        }
        else
        {
            Debug.LogError("Invalid input for diameter");
        }
    }

    public void SetCylinderDiameter(float diameter)
    {
        // Clear existing cylinders if any
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Calculate new positions and instantiate cylinders
        positions = CalculateHexagonPositions(hexagonSideLength, diameter);
        Vector3 cylinderScale = new Vector3(diameter, cylinderPrefab.transform.localScale.y, diameter); // Scale cylinder based on diameter

        foreach (Vector3 pos in positions)
        {
            GameObject cylinder = Instantiate(cylinderPrefab, pos, Quaternion.identity, transform);
            cylinder.transform.localScale = cylinderScale; // Apply the new scale
        }
    }
    bool IsCircleInsideHexagon(Vector2 center, float radius)
    {
        int samplePoints = 12; // Number of points around the circle to check
        for (int i = 0; i < samplePoints; i++)
        {
            float angle = 2 * Mathf.PI / samplePoints * i;
            Vector2 pointOnCircumference = new Vector2(
                center.x + radius * Mathf.Cos(angle),
                center.y + radius * Mathf.Sin(angle)
            );

            if (!IsPointInsideHexagon(pointOnCircumference))
            {
                return false;
            }
        }
        return true;
    }

    bool IsPointInsideHexagon(Vector2 point)
    {
        int insideAngles = 0;
        for (int i = 0; i < hexVertices.Count; i++)
        {
            Vector2 a = hexVertices[i];
            Vector2 b = hexVertices[(i + 1) % hexVertices.Count];

            Vector2 toA = a - point;
            Vector2 toB = b - point;

            float crossProduct = toA.x * toB.y - toB.x * toA.y;

            if (crossProduct > 0)
            {
                insideAngles++;
            }
        }

        return insideAngles == hexVertices.Count;
    }
    List<Vector3> CalculateHexagonPositions(float hexagonSideLength, float cylinderDiameter)
    {
        List<Vector3> positions = new List<Vector3>();
        float radius = cylinderDiameter / 2f;
        float verticalSpacing = Mathf.Sqrt(3) * radius;
        float horizontalSpacing = cylinderDiameter;

        int numRows = Mathf.CeilToInt(hexagonSideLength / verticalSpacing);
        for (int row = -numRows; row <= numRows; row++)
        {
            float z = row * verticalSpacing;
            bool isOffsetRow = Mathf.Abs(row) % 2 == 1;
            float xOffset = isOffsetRow ? radius : 0f;

            // Adjust row width calculation to ensure cylinders fit within hexagon
            float rowWidth = 2 * (hexagonSideLength - Mathf.Abs(z) / Mathf.Sqrt(3));
            int numCylindersInRow = Mathf.FloorToInt(rowWidth / horizontalSpacing);

            for (int col = -numCylindersInRow / 2; col <= numCylindersInRow / 2; col++)
            {
                float x = col * horizontalSpacing + xOffset;
                Vector3 pos = new Vector3(x, 0, z);
                if (IsCircleInsideHexagon(new Vector2(pos.x, pos.z), radius))
                {
                    positions.Add(pos);
                }
            }
        }

        return positions;
    }
    private void CalculateHexagonVertices()
    {
        hexVertices.Clear();
        for (int i = 0; i < 6; i++)
        {
            float angle_deg = 60 * i + 30; // Start from the middle of a side for horizontal orientation
            float angle_rad = Mathf.PI / 180 * angle_deg;
            hexVertices.Add(new Vector2(hexagonSideLength * Mathf.Cos(angle_rad),
                                        hexagonSideLength * Mathf.Sin(angle_rad)));
        }
    }
    void OnDrawGizmos()
    {
        float cylinderDiameter = cylinderPrefab.transform.localScale.x;
        float radius = cylinderDiameter / 2f; // Define radius here

        CalculateHexagonVertices(); // Update hexagon vertices calculation

        // Draw the hexagon
        Gizmos.color = Color.red;
        for (int i = 0; i < hexVertices.Count; i++)
        {
            Vector2 currentVertex = hexVertices[i];
            Vector2 nextVertex = hexVertices[(i + 1) % hexVertices.Count];
            Gizmos.DrawLine(new Vector3(currentVertex.x, 0, currentVertex.y), new Vector3(nextVertex.x, 0, nextVertex.y));
        }

        // Draw the staggered grid points
        Gizmos.color = Color.blue;
        float horizontalSpacing = cylinderDiameter;
        float verticalSpacing = Mathf.Sqrt(3) * radius;

        for (float z = -hexagonSideLength; z <= hexagonSideLength; z += verticalSpacing)
        {
            bool isOffsetRow = Mathf.Abs(z / verticalSpacing) % 2 == 1;
            float xOffset = isOffsetRow ? radius : 0f;

            for (float x = -hexagonSideLength + xOffset; x <= hexagonSideLength; x += horizontalSpacing)
            {
                Gizmos.DrawSphere(new Vector3(x, 0, z), 0.1f);
            }
        }
    }

}
