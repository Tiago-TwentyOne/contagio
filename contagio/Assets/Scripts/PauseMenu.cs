using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject tutorialManger;

    //When the button pause is pressed show UI and pause the game
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    //When the button Resume is pressed hide UI and unpause the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    //When the button quit is pressed unpause the game and show menu
    public void Leave()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    //When the button reload is pressed reload scene
    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
}
