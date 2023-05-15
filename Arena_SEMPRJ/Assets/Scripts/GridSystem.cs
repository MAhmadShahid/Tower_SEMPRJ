using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* My Questions:
1. What happens if i click somewhere out of range for the grid system ?
2. What can i do to adjust grid cell width ?


> Current Working of this approach
    click at the invisible grid to spawn objects on it.
    user doesn't know where and what to click

> Desired Working:
    spawn a grid with its respective tiles and perform operation upon this visible grid
*/

public class GridSystem : MonoBehaviour
{
    [SerializeField] private int _rows;
    [SerializeField] private int _columns;
    [SerializeField] private GameObject _enemyPrefab; // can also use tile prefab to show the grid layout in game

    private bool[,] _occupiedCells;
    private Vector3 _spawnPosition;

    private void Start()
    {
        // Initialize occupied cells array
        // Only one game object will be able to occupy a cell
        _occupiedCells = new bool[_rows, _columns];
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to mouse position on ground
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // currently no mask exist, to tell it if it hit the ground where we established our grid
            if (Physics.Raycast(ray, out hit))
            {
                // Calculate grid cell position
                int row = Mathf.FloorToInt(hit.point.x);
                int col = Mathf.FloorToInt(hit.point.z);

                if (row >= _rows || row < 0 || col < 0|| col >= _columns)
                {
                    Debug.Log("Out of Range");
                    return;
                }

                // Check if cell is occupied
                if (!_occupiedCells[row, col])
                {
                    _spawnPosition = new Vector3(row + 0.5f, 0, col + 0.5f);

                    // Spawn enemy at grid cell position
                    SpawnEnemy(_spawnPosition);

                    // Mark cell as occupied
                    _occupiedCells[row, col] = true;
                }
            }
        }
    }

    private void SpawnEnemy(Vector3 position)
    {
        // Instantiate enemy at position
        GameObject enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);

        // Set enemy as child of this object
        enemy.transform.parent = transform;
        enemy.transform.localScale = Vector3.one;

        Debug.Log($"Spawning Tile at {position}");
    }
}

