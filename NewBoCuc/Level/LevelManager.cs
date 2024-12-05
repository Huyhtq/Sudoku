using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum GameMode
    {
        Play,
        Campaign
    }

    public int CurrentLevel { get; private set; }
    public int CurrentLives { get; private set; }
    public int RemainingHints { get; private set; }
    public int[,] CurrentGrid { get; private set; }
    public GameMode CurrentMode { get; private set; }
    public float CampaignStartTime { get; private set; }
    public float TotalTimeElapsed { get; private set; }
    public float CurrentLevelStartTime { get; private set; }

    private const int MaxLives = 3;
    private const int MaxHints = 3;

    public void Initialize(GameMode mode)
    {
        CurrentMode = mode;
        CurrentLevel = 1;
        CurrentLives = MaxLives;
        RemainingHints = MaxHints;
        TotalTimeElapsed = 0f;

        if (mode == GameMode.Campaign)
        {
            CampaignStartTime = Time.time;
        }

        StartNewLevel();
    }

    public void StartNewLevel()
    {
        CurrentGrid = Generator.GeneratePuzzle(GetDifficultyLevel());
        CurrentLevelStartTime = Time.time;
    }

    public void ResetCurrentLevel()
    {
        StartNewLevel();
    }

    public void LevelCompleted()
    {
        if (CurrentMode == GameMode.Campaign)
        {
            if (CurrentLevel % 3 == 0 || CurrentLevel % 5 == 0)
            {
                CurrentLives = Mathf.Min(CurrentLives + 1, MaxLives);
            }
        }

        CurrentLevel++;
        StartNewLevel();
    }

    public bool UseHint()
    {
        if (RemainingHints > 0)
        {
            RemainingHints--;
            return true;
        }

        return false;
    }

    public void LoseLife()
    {
        CurrentLives--;
        if (CurrentLives <= 0)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        if (CurrentMode == GameMode.Campaign)
        {
            TotalTimeElapsed = Time.time - CampaignStartTime;
        }

        UIManager.Instance.ShowMainMenu(CurrentMode == GameMode.Campaign ? TotalTimeElapsed : -1);
    }

    public Generator.DifficultyLevel GetDifficultyLevel()
    {
        if (CurrentMode == GameMode.Play)
        {
            switch (PlayerPrefs.GetString("PlayMode"))
            {
                case "Easy": return Generator.DifficultyLevel.EASY;
                case "Medium": return Generator.DifficultyLevel.MEDIUM;
                case "Hard": return Generator.DifficultyLevel.HARD;
                case "VeryHard": return Generator.DifficultyLevel.VERY_HARD;
                default: return Generator.DifficultyLevel.EASY; 
            }
        }
        else if (CurrentMode == GameMode.Campaign)
        {
            if (CurrentLevel <= 3) return Generator.DifficultyLevel.EASY;
            if (CurrentLevel <= 6) return Generator.DifficultyLevel.MEDIUM;
            if (CurrentLevel <= 9) return Generator.DifficultyLevel.HARD;
            return Generator.DifficultyLevel.EXTREME;
        }

        return Generator.DifficultyLevel.EASY;
    }
    public void LoadNextLevel()
    {
        LevelCompleted();
    }
}
