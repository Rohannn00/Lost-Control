using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public float gridSize = 135f; // Size of grid cells
    public int gridWidth = 10; // Number of grid lines in width
    public int gridHeight = 10; // Number of grid lines in height
    public Vector3 offset; // Offset to adjust grid position

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        // Draw vertical lines
        for (int x = -gridWidth; x <= gridWidth; x++)
        {
            Vector3 start = new Vector3(x * gridSize, -gridHeight * gridSize, 0) + offset;
            Vector3 end = new Vector3(x * gridSize, gridHeight * gridSize, 0) + offset;
            Gizmos.DrawLine(start, end);
        }

        // Draw horizontal lines
        for (int y = -gridHeight; y <= gridHeight; y++)
        {
            Vector3 start = new Vector3(-gridWidth * gridSize, y * gridSize, 0) + offset;
            Vector3 end = new Vector3(gridWidth * gridSize, y * gridSize, 0) + offset;
            Gizmos.DrawLine(start, end);
        }
    }
}