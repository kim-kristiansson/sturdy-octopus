using UnityEngine;
using System.Collections.Generic;

public class HexagonPacker : MonoBehaviour
{
    public float cylinderDiameter = 1f;
    public float hexagonSideLength = 5f;
    public GameObject cylinderPrefab;
    private readonly List<Vector2> hexVertices = new List<Vector2>();

    void Start()
    {
        float radius = cylinderDiameter / 2f;
        CalculateHexagonVertices();

        // Spacing between cylinder centers
        float horizontalSpacing = cylinderDiameter;
        float verticalSpacing = 0.75f * cylinderDiameter;

        // Cover the area of the hexagon with a grid of points
        for (float z = -hexagonSideLength; z <= hexagonSideLength; z += verticalSpacing)
        {
            bool isOffsetRow = Mathf.Abs(z / verticalSpacing) % 2 == 1;
            float xOffset = isOffsetRow ? radius : 0f;

            for (float x = -hexagonSideLength + xOffset; x <= hexagonSideLength; x += horizontalSpacing)
            {
                Vector3 pos = new Vector3(x, 0, z); // Y is set to 0, assuming flat terrain
                if (IsCircleInsideHexagon(new Vector2(pos.x, pos.z), radius))
                {
                    Instantiate(cylinderPrefab, pos, Quaternion.identity);
                }
            }
        }
    }

    void CalculateHexagonVertices()
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

    void OnDrawGizmos()
    {
        // Draw the hexagon
        Gizmos.color = Color.red;
        for (int i = 0; i < hexVertices.Count; i++)
        {
            Vector2 nextVertex = hexVertices[(i + 1) % hexVertices.Count];
            Gizmos.DrawLine(new Vector3(hexVertices[i].x, 0, hexVertices[i].y), new Vector3(nextVertex.x, 0, nextVertex.y));
        }

        // Draw the staggered grid points
        Gizmos.color = Color.blue;
        float radius = cylinderDiameter / 2f;
        float horizontalSpacing = cylinderDiameter;
        float verticalSpacing = 0.75f * cylinderDiameter;

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
