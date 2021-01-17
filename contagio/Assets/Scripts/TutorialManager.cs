using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] texts;
    private int pos = 0;

    public GameObject tutorialUI;
    public Text tutorialText; 

    //On Awake pause the game and show the tutorial
    private void Awake()
    {
        Time.timeScale = 0;
        PauseMenu.gameIsPaused = true;
        tutorialUI.SetActive(true);
        NextText();
    }

    //Show next text when the button continue is pressed
    public void NextText()
    {
        if(pos < texts.Length)
        {
            tutorialText.text = texts[pos];
            pos++;
        }
        else
        {
            //Hide tutorial UI and unpause the game
            tutorialUI.SetActive(false);
            Time.timeScale = 1;
            PauseMenu.gameIsPaused = false;
        }
        
    }

}
