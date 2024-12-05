using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    // Phương thức để tiếp tục game (quay lại DoGame)
    public void ContinueGame()
    {
        Time.timeScale = 1;  // Khôi phục thời gian khi tiếp tục game
        SceneManager.LoadScene("Pause");  // Thoát khỏi scene Pause và quay lại DoGame
    }

    // Phương thức để chơi lại game (quay lại DoGame và reset game)
    public void PlayAgain()
    {
        Time.timeScale = 1;  // Khôi phục thời gian
        SceneManager.LoadScene("DoGame");  // Quay lại scene DoGame
        // Logic reset game nếu cần
    }

    // Phương thức để bắt đầu game mới
    public void NewGame()
    {
        Time.timeScale = 1;  // Khôi phục thời gian
        SceneManager.LoadScene("MainMenu");  // Quay lại MainMenu
    }

    // Phương thức để thoát game    
    public void ExitGame()
    {
        Time.timeScale = 1;  // Khôi phục thời gian
        SceneManager.LoadScene("MainMenu");  // Quay lại Main Menu
    }

}
