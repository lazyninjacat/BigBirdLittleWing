using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu_UI : MonoBehaviour
{
   public void StartButton()
    {
        SceneManager.LoadScene("ALLMANAGERS");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void AboutButton()
    {
        SceneManager.LoadScene("AboutScene");
    }
}
