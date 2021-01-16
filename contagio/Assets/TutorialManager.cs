using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] texts;
    private int pos = 0;

    public GameObject tutorialUI;
    public Text tutorialText; 

    private void Awake()
    {
        Time.timeScale = 0;
        PauseMenu.gameIsPaused = true;
        NextText();
    }

    public void NextText()
    {
        if(pos < texts.Length)
        {
            tutorialUI.SetActive(true);
            tutorialText.text = texts[pos];
            pos++;
        }
        else
        {
            tutorialUI.SetActive(false);
            Time.timeScale = 1;
            PauseMenu.gameIsPaused = false;
        }
        
    }

}
