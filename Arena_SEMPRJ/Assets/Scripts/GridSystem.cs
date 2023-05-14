using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public int rows;
    public int columns;
    public GameObject enemyPrefab;

    private bool[,] occupiedCells;
    private Vector3 spawnPosition;

    private void Start()
    {
        // Initialize occupied cells array
        occupiedCells = new bool[rows, columns];
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to mouse position on ground
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Calculate grid cell position
                int row = Mathf.FloorToInt(hit.point.x);
                int col = Mathf.FloorToInt(hit.point.z);

                // Check if cell is occupied
                if (!occupiedCells[row, col])
                {
                    spawnPosition = new Vector3(row + 0.5f, 0, col + 0.5f);

                    // Spawn enemy at grid cell position
                    SpawnEnemy(spawnPosition);

                    // Mark cell as occupied
                    occupiedCells[row, col] = true;
                }
            }
        }
    }

    private void SpawnEnemy(Vector3 position)
    {
        // Instantiate enemy at position
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);

        // Set enemy as child of this object
        enemy.transform.parent = transform;
    }
}
