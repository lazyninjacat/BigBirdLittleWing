using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_UI : MonoBehaviour
{

    [SerializeField] GameObject PauseMenu;

    // Start is called before the first frame update


    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (PauseMenu.activeSelf)
            {
                PauseMenu.SetActive(false);
            }
            else
            {
                PauseMenu.SetActive(true);
            }
        }

        if (PauseMenu.activeSelf && Input.GetKeyDown("q key"))
        {
            SceneManager.LoadScene("StartMenuScene");
        }
    }
}
