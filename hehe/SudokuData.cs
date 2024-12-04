using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generator;

public class SudokuData : MonoBehaviour
{
    public Dictionary<string, int[,]> sudoku_game = new Dictionary<string, int[,]>();

    void Start()
    {
        sudoku_game.Add("Easy", Generator.GeneratePuzzle(DifficultyLevel.EASY));
        sudoku_game.Add("Medium", Generator.GeneratePuzzle(DifficultyLevel.MEDIUM));
        sudoku_game.Add("Hard", Generator.GeneratePuzzle(DifficultyLevel.HARD));
        sudoku_game.Add("Very Hard", Generator.GeneratePuzzle(DifficultyLevel.VERY_HARD));
        sudoku_game.Add("Custom", Generator.GeneratePuzzle(DifficultyLevel.CUSTOM));

        // Debug example to check the generated puzzles
        foreach (var key in sudoku_game.Keys)
        {
            Debug.Log($"{key} Puzzle:");
            PrintGrid(sudoku_game[key]);
        }
    }

    // Helper method to print the grid in Unity's Console
    private void PrintGrid(int[,] grid)
    {
        string gridString = "";
        for (int r = 0; r < grid.GetLength(0); r++)
        {
            for (int c = 0; c < grid.GetLength(1); c++)
            {
                gridString += grid[r, c] + " ";
            }
            gridString += "\n";
        }
        Debug.Log(gridString); 
    }
}
