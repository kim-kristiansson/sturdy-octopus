using UnityEngine;
using System.Collections.Generic;
using TMPro; // Include the TextMeshPro namespace
using UnityEngine.UI; // Include the UI namespace for Button

public class HexagonCylinderPacker : MonoBehaviour
{
    public GameObject cylinderPrefab;
    public GameObject hexagonPrefab;
    public TMP_InputField diameterInputField; // TMP input field for diameter
    public Button generateButton; // Button to trigger generation

    private List<Vector2> hexVertices = new List<Vector2>();
    private float hexagonDiagonalLength;
    private float hexagonSideLength;
    private float cylinderDiameter;
    private List<Vector3> potentialCylinderPositions = new List<Vector3>();


    void Start()
    {
        CalculateHexagonVertices();
        hexagonDiagonalLength = CalculateHexagonDiagonalLength();
        hexagonSideLength = hexagonDiagonalLength / Mathf.Sqrt(3);

        // Set up the button click event
        generateButton.onClick.AddListener(GenerateCylindersOnClick);
    }
    private void GenerateCylindersOnClick()
    {
        if (float.TryParse(diameterInputField.text, out float diameter))
        {
            Debug.Log("Parsed diameter: " + diameter);
            SetCylinderDiameter(diameter);
        }
        else
        {
            Debug.LogError("Invalid input for diameter");
        }
    }

    public void SetCylinderDiameter(float diameter)
    {
        Debug.Log("Setting cylinder diameter to: " + diameter);
        cylinderDiameter = diameter;
        GenerateCylinders();
    }
    private void CalculateHexagonVertices()
    {
        hexVertices.Clear();
        Vector3 hexagonCenter = hexagonPrefab.transform.position;
        float hexagonScale = hexagonPrefab.transform.localScale.x; // Assuming uniform scale

        for (int i = 0; i < 6; i++)
        {
            float angle_deg = 60 * i; // Adjust angle for hexagon orientation
            float angle_rad = Mathf.PI / 180 * angle_deg;
            Vector2 vertex = hexagonCenter + new Vector3(hexagonScale * Mathf.Cos(angle_rad), 0, hexagonScale * Mathf.Sin(angle_rad));
            hexVertices.Add(vertex);
            Debug.Log("Hexagon Vertex " + i + ": " + vertex);
        }
    }

    private float CalculateHexagonDiagonalLength()
    {
        float maxLength = 0f;
        for (int i = 0; i < hexVertices.Count; i++)
        {
            for (int j = i + 1; j < hexVertices.Count; j++)
            {
                float length = Vector2.Distance(hexVertices[i], hexVertices[j]);
                if (length > maxLength)
                    maxLength = length;
            }
        }
        return maxLength;
    }

    private void GenerateCylinders()
    {
        Debug.Log("Generating cylinders with diameter: " + cylinderDiameter);

        // Clear existing cylinders if any
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        potentialCylinderPositions.Clear();

        float radius = cylinderDiameter / 2f;
        float verticalSpacing = Mathf.Sqrt(3) * radius;
        float horizontalSpacing = cylinderDiameter;

        int numRows = Mathf.CeilToInt(hexagonSideLength / verticalSpacing);
        for (int row = -numRows; row <= numRows; row++)
        {
            float z = row * verticalSpacing;
            bool isOffsetRow = Mathf.Abs(row) % 2 == 1;
            float xOffset = isOffsetRow ? radius : 0f;

            float rowWidth = 2 * (hexagonSideLength - Mathf.Abs(z) / Mathf.Sqrt(3));
            int numCylindersInRow = Mathf.FloorToInt(rowWidth / horizontalSpacing);

            for (int col = -numCylindersInRow / 2; col <= numCylindersInRow / 2; col++)
            {
                float x = col * horizontalSpacing + xOffset;
                Vector3 pos = new Vector3(x, 0, z) + hexagonPrefab.transform.position;
                if (IsPointInsideHexagon(new Vector2(pos.x, pos.z)))
                {
                    potentialCylinderPositions.Add(pos); // Add the position to the list
                    Debug.Log("Instantiating cylinder at: " + pos);
                    GameObject cylinder = Instantiate(cylinderPrefab, pos, Quaternion.identity, transform);
                    cylinder.transform.localScale = new Vector3(cylinderDiameter, cylinderPrefab.transform.localScale.y, cylinderDiameter);
                }
                else
                {
                    Debug.Log("Position outside hexagon: " + pos);
                }
            }
        }

        Debug.Log("Finished generating cylinders.");
    }


    private bool IsPointInsideHexagon(Vector2 point)
    {
        int intersections = 0;
        for (int i = 0; i < hexVertices.Count; i++)
        {
            Vector2 start = hexVertices[i];
            Vector2 end = hexVertices[(i + 1) % hexVertices.Count];

            if (IsLineIntersectingWithHorizontalRay(start, end, point))
                intersections++;
        }
        // If the number of intersections is odd, the point is inside
        return intersections % 2 != 0;
    }

    private bool IsLineIntersectingWithHorizontalRay(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
    {
        // Check if point is within y bounds of the line segment
        if (point.y < Mathf.Min(lineStart.y, lineEnd.y) || point.y > Mathf.Max(lineStart.y, lineEnd.y))
            return false;

        // Calculate x coordinate where line intersects with horizontal ray from the point
        float xIntersection = lineStart.x + (point.y - lineStart.y) * (lineEnd.x - lineStart.x) / (lineEnd.y - lineStart.y);

        // Check if this x coordinate is to the right of the point
        return xIntersection >= point.x;
    }
    void OnDrawGizmos()
    {
        // Draw hexagon vertices
        Gizmos.color = Color.red;
        foreach (var vertex in hexVertices)
        {
            Gizmos.DrawSphere(vertex, 0.1f); // Small red spheres at each vertex
        }

        // Draw potential cylinder positions
        Gizmos.color = Color.blue;
        foreach (var pos in potentialCylinderPositions)
        {
            Gizmos.DrawCube(pos, new Vector3(0.1f, 0.1f, 0.1f)); // Draw a small cube at each position
        }
    }

}
