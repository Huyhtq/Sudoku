using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _difficultyText;
    [SerializeField] private TMP_Text _hintText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private GameObject _pausePanel;

    public static UIManager Instance { get; private set; }

    public void Initialize()
    {
        UpdateLives(GameControl.Instance.LevelManager.CurrentLives);
        UpdateLevel(GameControl.Instance.LevelManager.CurrentLevel);
    }

    public void UpdateLives(int currentLives)
    {
        _livesText.text = $"Lives: {currentLives}/3";
    }

    public void UpdateLevel(int level)
    {
        if (GameControl.Instance.LevelManager.CurrentMode == LevelManager.GameMode.Campaign)
        {
            _levelText.text = $"LEVEL {level}";
        }
        else
        {
            _levelText.text = $"DIFFICULTY: {GameControl.Instance.LevelManager.GetDifficultyLevel()}";
        }
    }

    public void ShowPauseMenu(bool isPaused)
    {
        _pausePanel.SetActive(isPaused);
    }

    public void ShowMainMenu(float campaignTime = -1)
    {
        if (campaignTime >= 0)
        {
            Debug.Log($"Total Campaign Time: {campaignTime} seconds");
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void UpdateHints(int hints)
    {
        _hintText.text = $"Lives: {hints}/3";
    }

    public void UpdateTimer(float timeElapsed)
    {
        System.TimeSpan time = System.TimeSpan.FromSeconds(timeElapsed);
        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);

        _timerText.text = formattedTime;
    }

    public void ShowGameOverScreen()
    {
        Debug.Log("Game Over! Display Game Over screen.");

    }

}
