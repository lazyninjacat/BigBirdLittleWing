using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AboutScene_UI : MonoBehaviour
{
   public void BackButton()
    {
        SceneManager.LoadScene("StartMenuScene");
    }
}
