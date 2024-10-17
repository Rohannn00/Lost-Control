using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public GameObject prefab;  // Prefab to instantiate
    public float spacing = 0.5f; // Spacing between prefabs
    public float moveOffset = 1f; // Move each prefab by 1 unit
    public bool hidePrefabs = true; // Public bool to control prefab visibility
    public Transform player; // Reference to the player's Transform
    public bool followPlayer = false; // Public boolean to toggle following the player

    private Vector2 screenDimensions;
    private Vector2 prefabSize;
    private Vector2[,] prefabPositions; // 2D array to store prefab positions
    private List<GameObject> instantiatedPrefabs = new List<GameObject>(); // List to keep track of instantiated prefabs

    void Start()
    {
        // Get screen dimensions and prefab size
        screenDimensions = GetScreenDimensions();
        prefabSize = GetPrefabSize();

        // If prefab size could not be determined, stop execution
        if (prefabSize == Vector2.zero)
        {
            Debug.LogError("Prefab size could not be determined. Make sure the prefab has a valid component (SpriteRenderer, Collider2D, or RectTransform).");
            return;
        }

        // Calculate the number of columns and rows that fit on the screen
        int columns = Mathf.FloorToInt(screenDimensions.x / (prefabSize.x + spacing));
        int rows = Mathf.FloorToInt(screenDimensions.y / (prefabSize.y + spacing)) - 2; // Subtracting 2 to exclude the last two rows

        // Ensure there are no negative or zero rows
        rows = Mathf.Max(rows, 0);

        // Initialize 2D array to store prefab positions
        prefabPositions = new Vector2[rows, columns];

        // Create the grid of prefabs
        CreateGrid(rows, columns);
    }

    Vector2 GetScreenDimensions()
    {
        // Convert screen pixel size to world units in 2D
        Vector2 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));  // Bottom left corner
        Vector2 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)); // Top right corner

        // Return the full screen size in world units
        return new Vector2(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }

    Vector2 GetPrefabSize()
    {
        // Try to get the prefab size from SpriteRenderer
        if (prefab.GetComponent<SpriteRenderer>() != null)
        {
            SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
            return spriteRenderer.bounds.size;
        }
        // If no SpriteRenderer, try to get the prefab size from Collider2D (for other 2D objects)
        else if (prefab.GetComponent<Collider2D>() != null)
        {
            Collider2D collider = prefab.GetComponent<Collider2D>();
            return collider.bounds.size;
        }
        // If no Collider2D, try to get the prefab size from RectTransform (for UI elements)
        else if (prefab.GetComponent<RectTransform>() != null)
        {
            RectTransform rectTransform = prefab.GetComponent<RectTransform>();
            return rectTransform.rect.size;
        }
        else
        {
            Debug.LogError("Prefab does not have a SpriteRenderer, Collider2D, or RectTransform component.");
            return Vector2.zero;  // Return zero to indicate failure
        }
    }

    void CreateGrid(int rows, int columns)
    {
        // Start populating from the center of the screen
        Vector2 startPos = new Vector2(-(columns - 1) * (prefabSize.x + spacing) / 2, (rows - 1) * (prefabSize.y + spacing) / 2);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate position for each prefab in the grid (with spacing)
                Vector2 position = new Vector2(
                    startPos.x + col * (prefabSize.x + spacing),   // X position for columns
                    startPos.y - row * (prefabSize.y + spacing)    // Y position for rows
                );

                // Move prefab by the specified offset
                position += new Vector2(0, moveOffset); // Move upwards by 1 unit

                // Instantiate prefab at calculated position
                GameObject newPrefab = Instantiate(prefab, position, Quaternion.identity);

                // Set the grid creator as the parent of the new prefab
                newPrefab.transform.SetParent(transform); // Set the grid creator's transform as the parent

                // Store the position in the 2D array
                prefabPositions[row, col] = position;

                // Add the new prefab to the list
                instantiatedPrefabs.Add(newPrefab);

                // Hide or show the prefab based on the public bool
                SpriteRenderer spriteRenderer = newPrefab.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = !hidePrefabs; // Hide if hidePrefabs is true
                }
            }
        }
    }

    void Update()
    {
        if (followPlayer)
        {
            // Move the GridCreator object to the player's position
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
    }
}
