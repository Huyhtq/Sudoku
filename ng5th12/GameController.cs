using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private Generator.DifficultyLevel gameMode;

    void Start()
    {
        // Lấy giá trị chế độ chơi đã lưu từ PlayerPrefs (mặc định là "Easy" nếu không có)
        string storedGameMode = PlayerPrefs.GetString("GameMode", "Easy");
        // Chuyển đổi chế độ chơi từ chuỗi sang enum DifficultyLevel
        gameMode = (Generator.DifficultyLevel)System.Enum.Parse(typeof(Generator.DifficultyLevel), storedGameMode);

        // Gọi hàm để tạo một game mới với difficulty đã chọn
        CreateNewGame(gameMode);
    }

    private void CreateNewGame(Generator.DifficultyLevel difficulty)
    {
        // Tạo puzzle mới từ Generator
        int[,] puzzle = Generator.GeneratePuzzle(difficulty);

        // Lưu puzzle vào PlayerPrefs (để tiếp tục trò chơi sau)
        SavePuzzleToPlayerPrefs(puzzle);
    }

    private void SavePuzzleToPlayerPrefs(int[,] puzzle)
    {
        string gridString = "";
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                gridString += puzzle[i, j] + ",";
            }
        }
        gridString = gridString.TrimEnd(',');  // Loại bỏ dấu phẩy cuối cùng
        PlayerPrefs.SetString("Grid", gridString); // Lưu vào PlayerPrefs
    }

    public void SetGameMode(Generator.DifficultyLevel mode)
    {
        gameMode = mode;
        PlayerPrefs.SetString("GameMode", mode.ToString());
    }

    public void LoadNextLevel()
    {
        // Tăng level và tạo game mới
        int currentLevel = PlayerPrefs.GetInt("Level", 0);
        int nextLevel = currentLevel + 1;
        PlayerPrefs.SetInt("Level", nextLevel);

        // Chuyển sang scene tiếp theo (hoặc reset level nếu đã kết thúc)
        SceneManager.LoadScene("DoGame");
    }

    public void RestartGame()
    {
        // Quay lại scene ban đầu, reset tất cả các thông tin trò chơi
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("StartMenu");
    }
}
