using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject PauseMenuPanel;
    private bool pauseMenuIsOpen;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuIsOpen = false;
        PauseMenuPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if (pauseMenuIsOpen)
            {
                PauseMenuPanel.SetActive(false);
                pauseMenuIsOpen = false;
            }
            else
            {
                PauseMenuPanel.SetActive(true);
                pauseMenuIsOpen = true;

            }
        }
    }
}
