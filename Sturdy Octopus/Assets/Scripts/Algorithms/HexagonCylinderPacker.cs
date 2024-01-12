using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class HexagonCylinderPacker : MonoBehaviour
{
    public TMP_InputField diameterInputField;
    public TMP_InputField lengthInputField;
    public Button generateButton;
    public Cylinder cylinderPrefab;
    public Hexagon hexagonPrefab;

    private Hexagon generatedHexagon;
    private List<Cylinder> generatedCylinders = new List<Cylinder>();

    private void Start()
    {
        generateButton.onClick.AddListener(OnGenerateClicked);
    }

    private void OnGenerateClicked()
    {
        float diameter = float.Parse(diameterInputField.text);
        float length = float.Parse(lengthInputField.text);

        if (diameter <= 0 || length <= 0)
        {
            Debug.LogError("Invalid dimensions. Diameter and Length must be positive.");
            return;
        }

        ClearExistingCylinders();
        GenerateHexagonIfNeeded();

        GenerateCylindersInsideHexagon(diameter, length);
    }

    private void ClearExistingCylinders()
    {
        foreach (var cylinder in generatedCylinders)
        {
            Destroy(cylinder.gameObject);
        }
        generatedCylinders.Clear();
    }

    private void GenerateHexagonIfNeeded()
    {
        if (generatedHexagon == null)
        {
            generatedHexagon = Instantiate(hexagonPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            // Optionally reset or clear the existing hexagon
        }
    }
    private float CalculateHexagonApothem(float sideLength)
    {
        return sideLength * Mathf.Sqrt(3) / 2;
    }

    private void GenerateCylindersInsideHexagon(float diameter, float length)
    {
        float cylinderRadius = diameter / 2f;
        float hexagonApothem = CalculateHexagonApothem(generatedHexagon.SideLength);

        // Calculate rows based on hexagon's height
        int numRows = Mathf.FloorToInt((2 * hexagonApothem) / (Mathf.Sqrt(3) * cylinderRadius));
        float hexagonWidth = generatedHexagon.SideLength * 2;

        for (int row = 0; row < numRows; row++)
        {
            bool isOddRow = (row % 2) == 1;
            float xOffset = isOddRow ? cylinderRadius : 0;

            for (float x = -hexagonWidth / 2; x < hexagonWidth / 2; x += diameter)
            {
                Vector3 position = new Vector3(x + xOffset, 0, row * Mathf.Sqrt(3) * cylinderRadius - hexagonApothem);
                if (IsCylinderWithinHexagonBounds(position, cylinderRadius, generatedHexagon.SideLength))
                {
                    Cylinder newCylinder = Instantiate(cylinderPrefab, position, Quaternion.identity);
                    newCylinder.SetPhysicalProperties(length, diameter, 1.0f);
                    generatedCylinders.Add(newCylinder);
                }
            }
        }
    }

    private bool IsCylinderWithinHexagonBounds(Vector3 cylinderCenter, float cylinderRadius, float hexagonSideLength)
    {
        // Calculate the corners of the hexagon
        Vector2[] hexagonCorners = new Vector2[6];
        for (int i = 0; i < 6; i++)
        {
            float angle_deg = 60 * i + 30;  // Start flat side at the bottom
            float angle_rad = Mathf.PI / 180 * angle_deg;
            hexagonCorners[i] = new Vector2(hexagonSideLength * Mathf.Cos(angle_rad), hexagonSideLength * Mathf.Sin(angle_rad));
        }

        // Check if the cylinder is inside all six triangles
        for (int i = 0; i < 6; i++)
        {
            Vector2 corner1 = hexagonCorners[i];
            Vector2 corner2 = hexagonCorners[(i + 1) % 6];

            // If any of the checks fail, the cylinder is outside the hexagon
            if (!IsPointInsideTriangle(Vector2.zero, corner1, corner2, new Vector2(cylinderCenter.x, cylinderCenter.z)))
            {
                return false;
            }
        }

        return true;
    }

    private bool IsPointInsideTriangle(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        // A point is inside a triangle if it is on the same side of all the triangle's edges.
        var s = p0.y * p2.x - p0.x * p2.y + (p2.y - p0.y) * p.x + (p0.x - p2.x) * p.y;
        var t = p0.x * p1.y - p0.y * p1.x + (p0.y - p1.y) * p.x + (p1.x - p0.x) * p.y;

        if ((s < 0) != (t < 0))
            return false;

        var area = -p1.y * p2.x + p0.y * (p2.x - p1.x) + p0.x * (p1.y - p2.y) + p1.x * p2.y;
        return area < 0 ? (s <= 0 && s + t >= area) : (s >= 0 && s + t <= area);
    }

    void OnDrawGizmos()
    {
        // Draw the hexagon boundary
        if (generatedHexagon != null)
        {
            Gizmos.color = Color.yellow;
            DrawHexagon(generatedHexagon.transform.position, generatedHexagon.SideLength);
        }

        // Draw cylinder positions
        Gizmos.color = Color.green;
        foreach (var cylinder in generatedCylinders)
        {
            Gizmos.DrawWireSphere(cylinder.transform.position, cylinder.Diameter / 2);
        }
    }

    private void DrawHexagon(Vector3 center, float sideLength)
    {
        Vector3[] vertices = new Vector3[7];
        for (int i = 0; i < 6; i++)
        {
            float angle_deg = 60 * i + 30;
            float angle_rad = Mathf.PI / 180 * angle_deg;
            vertices[i] = new Vector3(center.x + sideLength * Mathf.Cos(angle_rad),
                                      center.y,
                                      center.z + sideLength * Mathf.Sin(angle_rad));
        }
        vertices[6] = vertices[0]; // Close the hexagon

        for (int i = 0; i < 6; i++)
        {
            Gizmos.DrawLine(vertices[i], vertices[i + 1]);
        }
    }

}
