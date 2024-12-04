using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Generator;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject Menu_Panel;
    [SerializeField] private GameObject playButton; // Tham chiếu tới nút Play
    [SerializeField] private List<GameObject> menuItems;

    public void LoadGame(string difficulty)
    {
        DifficultyLevel selectedDifficulty;

        switch (difficulty)
        {
            case "Easy":
                selectedDifficulty = DifficultyLevel.EASY;
                break;
            case "Medium":
                selectedDifficulty = DifficultyLevel.MEDIUM;
                break;
            case "Hard":
                selectedDifficulty = DifficultyLevel.HARD;
                break;
            case "Very Hard":
                selectedDifficulty = DifficultyLevel.VERY_HARD;
                break;
            case "Custom":
                selectedDifficulty = DifficultyLevel.CUSTOM;
                break;
            default:
                Debug.LogError("Invalid difficulty level selected");
                return;
        }

        Generator.GeneratePuzzle(selectedDifficulty);
        SceneManager.LoadScene("GameScene"); // Replace "GameScene" with the actual name of your game scene
    }

    public void DeActivateObject()
    {
        if (Menu_Panel != null)
            Menu_Panel.SetActive(false);

        if (playButton != null)
            playButton.SetActive(true);
    }
    public void ActivateObject()
{
        if (Menu_Panel != null)
        {
            Menu_Panel.SetActive(true);
            playButton.SetActive(false);
        }

    }
    public void OpenScoreBoard()
    {
        SceneManager.LoadScene("Scoreboard"); // Thay "Scoreboard" bằng tên scene bạn tạo
    }
}
