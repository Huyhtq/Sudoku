using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private SubGrid _subGridPrefab;
    [SerializeField] private Transform gridParent;
    private Cell[,] cells;
    private const int GridSize = 9;

    public void Initialize(int[,] puzzleGrid)
    {
        ClearGrid();
        cells = new Cell[GridSize, GridSize];
        SpawnGrid(puzzleGrid);
    }

    private void SpawnGrid(int[,] puzzleGrid)
    {
        for (int i = 0; i < GridSize; i++)
        {
            Vector3 spawnPos = CalculateSpawnPosition(i);
            SubGrid subGrid = Instantiate(_subGridPrefab, spawnPos, Quaternion.identity, gridParent);

            for (int j = 0; j < GridSize; j++)
            {
                Cell cell = subGrid.cells[j];
                int row = (i / 3) * 3 + j / 3;
                int col = (i % 3) * 3 + j % 3;
                cell.Init(puzzleGrid[row, col], row, col);
                cells[row, col] = cell;
            }
        }
    }

    private Vector3 CalculateSpawnPosition(int index)
    {
        return new Vector3(index % 3, index / 3, 0); 
    }

    private void ClearGrid()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
    }
}
