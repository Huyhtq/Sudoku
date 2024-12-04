using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Generator;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject Menu_Panel;

      
   public void LoadScene(string name)
    {
        SceneManager.LoadScene(name); 
    }

    public void LoadEasyGame(string name)
    {
        Generator.GeneratePuzzle(DifficultyLevel.EASY);
        SceneManager.LoadScene(name);
    }

    public void LoadMediumGame(string name)
    {
        Generator.GeneratePuzzle(DifficultyLevel.MEDIUM);
        SceneManager.LoadScene(name);
    }

    public void LoadHardGame(string name)
    {
        Generator.GeneratePuzzle(DifficultyLevel.HARD);
        SceneManager.LoadScene(name);
    }
    public void LoadVeryHardGame(string name)
    {
        Generator.GeneratePuzzle(DifficultyLevel.VERY_HARD);
        SceneManager.LoadScene(name);
    }
    public void LoadCustomGame(string name)
    {
        Generator.GeneratePuzzle(DifficultyLevel.CUSTOM);  
        SceneManager.LoadScene(name);
    }



    public void ActivateObject()
    {
        Menu_Panel.SetActive(true);
    }
    public void DeActivateObject()
    {
        Menu_Panel.SetActive(false);
    }
}
