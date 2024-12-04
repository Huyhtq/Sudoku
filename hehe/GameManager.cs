using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private float _offsetX, _offsetY;
    [SerializeField] private SubGrid _subGridPrefab;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private TMP_Text _difficultyText;
    [SerializeField] private GameObject _confirmationPanel;


    private int maxLive = 3;
    private int currentLives;
    private bool hasGameFinished;
    private Cell[,] cells;
    private Cell selectedCell;

    private const int GridSize = 9;
    private const int SubGridSize= 3;

    private void Start()
    {
        /*hasGameFinished = false;
        currentLives = maxLive;
        UpdateLivesUI();
        cells = new Cell[GridSize, GridSize];
        selectedCell = null;
        SpawnCells();*/
        // Lấy giá trị chế độ chơi đã lưu trong PlayerPrefs
        // Lấy chế độ chơi từ PlayerPrefs (mặc định là "Easy" nếu không có giá trị)
        string gameMode = PlayerPrefs.GetString("GameMode", "Easy");

        // Chuyển đổi chuỗi thành giá trị enum DifficultyLevel
        Generator.DifficultyLevel difficulty = (Generator.DifficultyLevel)System.Enum.Parse(typeof(Generator.DifficultyLevel), gameMode);

        // In ra chế độ chơi đã chọn để kiểm tra
        Debug.Log("Game mode: " + gameMode);

        // Gọi hàm để tạo game mới với difficulty đã chọn
        CreateNewGame(difficulty);

        // Các thao tác khởi tạo khác cho GameManager
        hasGameFinished = false;
        currentLives = maxLive;
        UpdateLivesUI();
        cells = new Cell[GridSize, GridSize];
        selectedCell = null;
        SpawnCells();
    }

    // Giả sử bạn có hàm CreateNewGame như thế này
    private void CreateNewGame(Generator.DifficultyLevel difficulty)
    {
        // Logic để tạo game mới dựa trên chế độ difficulty
        Debug.Log("Creating new game with difficulty: " + difficulty);
        // Bạn có thể sử dụng difficulty để tạo bảng Sudoku hoặc cấu hình game

    }

    // Phương thức tạo game mới, bạn có thể viết logic riêng tùy thuộc vào cách bạn muốn tạo bảng trò chơi
   /* private void CreateNewGame(Generator.DifficultyLevel difficulty)
        {
            // Logic tạo bảng mới với difficulty, ví dụ:
            int[,] puzzleGrid = Generator.GeneratePuzzle(difficulty);

            // Thực hiện các hành động khác với puzzleGrid...
            // Ví dụ: Hiển thị bảng lên UI, tạo các đối tượng cell, v.v.
        }
   */
    private void UpdateLivesUI()
    {
        _livesText.text = "Lives: " + currentLives + "/3";
    }

    public void UpdateCellValue(int value)
    {
        if (hasGameFinished || selectedCell == null) return;

        selectedCell.UpdateValue(value);

        if (!IsValid(selectedCell, cells)) 
        {
            currentLives--; 
            UpdateLivesUI();

            if (currentLives <= 0) 
            {
                RestartGame(); 
                return;
            }
        }

        HighLight();
        CheckWin();
    }

    private void SpawnCells()
    {
        int[,] puzzleGrid = new int[GridSize, GridSize];
        int level = PlayerPrefs.GetInt("Level", 0);

        if (level == 0)
        {
            CreateAndStoreLevel(puzzleGrid, 1);
            level = 1;
        }
        else
        {
            GetCurrentLevel(puzzleGrid);
        }

        _levelText.text = "LEVEL " + level.ToString();
        _difficultyText.text = "DIFFICULTY: " + GetDifficulty(level);

        for (int i = 0; i < GridSize; i++)
        {
            Vector3 spawnPos = _startPos + i % 3 * _offsetX * Vector3.right + i / 3 * _offsetY * Vector3.up;
            SubGrid subGrid = Instantiate(_subGridPrefab, spawnPos, Quaternion.identity);
            List<Cell> subgridCells = subGrid.cells;
            int startRow = (i / 3) * 3;
            int startCol = (i % 3) * 3;
            for (int j = 0; j < GridSize; j++)
            {
                subgridCells[j].Row = startRow + j / 3;
                subgridCells[j].Col = startCol + j % 3;
                int cellValue = puzzleGrid[subgridCells[j].Row, subgridCells[j].Col];
                subgridCells[j].Init(cellValue);
                cells[subgridCells[j].Row, subgridCells[j].Col] = subgridCells[j];
            }
        }
    }

    private void CreateAndStoreLevel(int[,] grid, int level)
    {
        int[,] tempGrid = Generator.GeneratePuzzle((Generator.DifficultyLevel)(level / 5));
        string arrayString = "";
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                arrayString += tempGrid[i, j].ToString() + ",";
                grid[i, j] = tempGrid[i, j];
            }
        }

        arrayString = arrayString.TrimEnd(',');
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetString("Grid", arrayString);
    }

    private void GetCurrentLevel(int[,] grid)
    {
        string arrayString = PlayerPrefs.GetString("Grid");
        string[] arrayValue = arrayString.Split(',');
        int index = 0;
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                grid[i, j] = int.Parse(arrayValue[index]);
                index++;
            }
        }
    }

    private void GoToNextLevel()
    {
        int level = PlayerPrefs.GetInt("Level", 0);
        CreateAndStoreLevel(new int[GridSize, GridSize], level + 1);
        RestartGame();
    }

    private void Update()
    {
        if (hasGameFinished || !Input.GetMouseButton(0)) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        Cell tempCell;

        if (hit
            && hit.collider.gameObject.TryGetComponent(out tempCell)
            && tempCell != selectedCell
            && !tempCell.IsLocked
            )
        {
            ResetGrid();
            selectedCell = tempCell;
            HighLight();
        }
    }

    private void ResetGrid()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                cells[i, j].Reset();
            }
        }
    }

    private void CheckWin()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if (cells[i, j].IsIncorrect || cells[i, j].Value == 0) return;
            }
        }

        hasGameFinished = true;

        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                cells[i, j].UpdateWin();
            }
        }

        Invoke("GoToNextLevel", 2f);
    }

    private void HighLight()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                cells[i, j].IsIncorrect = !IsValid(cells[i, j], cells);
            }
        }

        int currentRow = selectedCell.Row;
        int currentCol = selectedCell.Col;
        int subGridRow = currentRow - currentRow % SubGridSize;
        int subGridCol = currentCol - currentCol % SubGridSize;

        for (int i = 0; i < GridSize; i++)
        {
            cells[i, currentCol].HighLight();
            cells[currentRow, i].HighLight();
            cells[subGridRow + i % 3, subGridCol + i / 3].HighLight();
        }

        cells[currentRow, currentCol].Select();
    }

    private bool IsValid(Cell cell, Cell[,] cells)
    {
        int row = cell.Row;
        int col = cell.Col;
        int value = cell.Value;
        cell.Value = 0;

        if (value == 0) return true;

        for (int i = 0; i < GridSize; i++)
        {
            if (cells[row, i].Value == value || cells[i, col].Value == value)
            {
                cell.Value = value;
                return false;
            }
        }

        int subGridRow = row - row % SubGridSize;
        int subGridCol = col - col % SubGridSize;

        for (int r = subGridRow; r < subGridRow + SubGridSize; r++)
        {
            for (int c = subGridCol; c < subGridCol + SubGridSize; c++)
            {
                if (cells[r, c].Value == value)
                {
                    cell.Value = value;
                    return false;
                }
            }
        }

        cell.Value = value;
        return true;
    }

    public void RestartGame()
    {
        _livesText.text = "Game Over! Restarting...";
        Invoke(nameof(ResetLevel), 2f);
    }

    private void ResetLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private string GetDifficulty(int level)
    {
        if (level <= 3) return "Easy";
        if (level <= 6) return "Medium";
        if (level <= 9) return "Hard";
        if (level <= 12) return "Super Hard";
        if (level <= 15) return "Extreme Hard!!!";
        return "Random";
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); 
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
    }

    public void ShowResetConfirmation()
    {
        _confirmationPanel.SetActive(true);
    }

    public void ConfirmReset()
    {
        ResetProgress();
        _confirmationPanel.SetActive(false);
    }

    public void CancelReset()
    {
        _confirmationPanel.SetActive(false);
    }
}