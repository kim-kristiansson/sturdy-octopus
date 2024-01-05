using UnityEngine;
using System.Collections.Generic;

public class HexagonCylinderPacker : MonoBehaviour
{
    public GameObject cylinderPrefab;
    public float cylinderDiameter = 1f;
    public GameObject hexagonPrefab;

    private List<Vector2> hexVertices = new List<Vector2>();
    private float hexagonSideLength;

    void Start()
    {
        hexagonSideLength = CalculateHexagonSideLength();
        CalculateHexagonVertices();
        GenerateCylinders();
    }

    private float CalculateHexagonSideLength()
    {
        // Assuming the hexagon's side length is represented by its scale.x
        return hexagonPrefab.transform.localScale.x;
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

    private void GenerateCylinders()
    {
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
                    Instantiate(cylinderPrefab, pos, Quaternion.identity, transform);
                }
            }
        }
    }

    private bool IsPointInsideHexagon(Vector2 point)
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
                insideAngles++;
        }
        return insideAngles == hexVertices.Count;
    }
}
