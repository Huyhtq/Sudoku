
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Generator
{
    public enum DifficultyLevel
    {
        EASY,//0
        MEDIUM,//1
        HARD,//2
        VERY_HARD,//3 
        CUSTOM //4

    }

    private const int GridSize = 9;
    private const int SubGridSize = 3;
    private const int BoardSize = 9;
    private const int MinSquaredRemoved = 30;
    private const int MaxSquaredRemoved = 70;

    public static int[,] GeneratePuzzle(DifficultyLevel difficultyLevel)
    {
        var grid = new int[GridSize, GridSize];
        int squaresToRemove = 0;

        switch (difficultyLevel)
        {
            case DifficultyLevel.EASY:
                squaresToRemove = Random.Range(MinSquaredRemoved, MinSquaredRemoved + 10); // Easy
                break;
            case DifficultyLevel.MEDIUM:
                squaresToRemove = Random.Range(MinSquaredRemoved + 11, MinSquaredRemoved + 20); // Medium
                break;
            case DifficultyLevel.HARD:
                squaresToRemove = Random.Range(MinSquaredRemoved + 21, MinSquaredRemoved + 30); // Difficult
                break;
            case DifficultyLevel.VERY_HARD:
                squaresToRemove = Random.Range(MinSquaredRemoved + 31, MinSquaredRemoved + 40); // Very Difficult
                break;
            case DifficultyLevel.CUSTOM:
                squaresToRemove = Random.Range(MinSquaredRemoved + 41, MaxSquaredRemoved); // Extreme
                break;
            default:
                // Random difficult
                DifficultyLevel randomDifficulty = (DifficultyLevel)Random.Range(0, 5);
                return GeneratePuzzle(randomDifficulty);
        }

        InitializeGrid(grid);

        while (squaresToRemove > 0)
        {
            int randRow = Random.Range(0, BoardSize);
            int randCol = Random.Range(0, BoardSize);

            if (grid[randRow, randCol] != 0)
            {
                int temp = grid[randRow, randCol];
                grid[randRow, randCol] = 0;

                if (Solver.HasUniqueSolution(grid))
                {
                    squaresToRemove--;
                }
                else
                {
                    grid[randRow, randCol] = temp;
                }
            }
        }

        return grid;
    }

    public static void InitializeGrid(int[,] grid)
    {
        List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Shuffle(numbers);
        for (int i = 0; i < GridSize; i++)
        {
            grid[0, i] = numbers[i];
        }
        FillGrid(1, 0, grid);
    }

    private static bool FillGrid(int r, int c, int[,] grid)
    {
        if (r == GridSize)
        {
            return true;
        }
        if (c == GridSize)
        {
            return FillGrid(r + 1, 0, grid);
        }

        List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Shuffle(numbers);
        foreach (var num in numbers)
        {
            if (IsValid(num, r, c, grid))
            {
                grid[r, c] = num;

                if (FillGrid(r, c + 1, grid))
                {
                    return true;
                }
            }
        }

        grid[r, c] = 0;
        return false;
    }

    private static bool IsValid(int val, int row, int col, int[,] board)
    {

        for (int i = 0; i < BoardSize; i++)
        {
            if (board[row, i] == val)
            {
                return false;
            }
        }

        for (int i = 0; i < BoardSize; i++)
        {
            if (board[i, col] == val)
            {
                return false;
            }
        }

        int subGridRow = row / SubGridSize * SubGridSize;
        int subGridCol = col / SubGridSize * SubGridSize;
        for (int r = subGridRow; r < subGridRow + SubGridSize; r++)
        {
            for (int c = subGridCol; c < subGridCol + SubGridSize; c++)
            {
                if (board[r, c] == val)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    private DifficultyLevel GameMode;

    void Start()
    {
        GameMode = DifficultyLevel.CUSTOM;
    }

    public void SetGameMode(DifficultyLevel mode)
    {
        GameMode = mode;
    }

    public void SetGameMode(string mode)
    {
        if (mode == "Easy") SetGameMode(DifficultyLevel.EASY);
        else if (mode == "Medium") SetGameMode(DifficultyLevel.MEDIUM);
        else if (mode == "Hard") SetGameMode(DifficultyLevel.HARD);
        else if (mode == "Very Hard") SetGameMode(DifficultyLevel.VERY_HARD);
        else  SetGameMode(DifficultyLevel.CUSTOM);
    }
    public void SetGameModeAndStart(string mode)
    {
        // Thiết lập chế độ chơi trong Generator
        Generator generator = new Generator();
        generator.SetGameMode(mode);  // Cập nhật chế độ chơi

        // Lưu chế độ chơi vào PlayerPrefs để lấy sau khi chuyển scene
        PlayerPrefs.SetString("GameMode", mode);

        // Chuyển sang scene DoGame
        UnityEngine.SceneManagement.SceneManager.LoadScene("DoGame");
    }


    public string GetGameMode()
    {
        switch (GameMode) {
            case DifficultyLevel.EASY: return "Easy";
            case DifficultyLevel.MEDIUM: return "Medium";
            case DifficultyLevel.HARD: return "Hard";
            case DifficultyLevel.VERY_HARD: return "Very Hard";
            case DifficultyLevel.CUSTOM: return "Custom";
        }
        Debug.LogError("Error: Game level is not set");
        return " ";
    }
}


