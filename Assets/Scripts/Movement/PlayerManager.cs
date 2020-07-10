using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] PlayerController[] players;
    [SerializeField] int currentPlayerIndex = 0;
    [SerializeField] KeyCode switchPlayers = KeyCode.Space;

    [SerializeField] GameObject BigBirdCam;
    [SerializeField] GameObject LittleWingCam;

    private Camera mainCamera;
    public PlayerController currentSub;

    private void SwapPlayer()
    {
        //stop rb of previous player
        currentSub.rb.velocity = Vector3.zero;

        //just move through array normally
        currentPlayerIndex = (currentPlayerIndex + 1 + players.Length) % players.Length;
        currentSub = players[currentPlayerIndex];
        //CameraSetup();
        SwapActiveCamera();
    }

    private void SwapActiveCamera()
    {
        if (BigBirdCam.activeSelf)
        {
            BigBirdCam.SetActive(false);
            LittleWingCam.SetActive(true);
        }
        else
        {
            LittleWingCam.SetActive(false);
            BigBirdCam.SetActive(true);
        }
    }


    //public void CameraSetup()
    //{
    //    mainCamera.transform.parent = currentSub.cameraPosition;
    //    mainCamera.transform.localPosition = Vector3.zero;
    //    mainCamera.transform.rotation = currentSub.transform.rotation;
    //}

    private void Start()
    {
        mainCamera = Camera.main;
        currentSub = players[currentPlayerIndex];
        //CameraSetup();
        SwapActiveCamera();
    }

    private void Update()
    {
        //check for sub switch here
        if(Input.GetKeyUp(switchPlayers))
        {
            SwapPlayer();
        }
        else
        {
            currentSub.RunUpdate();
        }

      
    }

    private void FixedUpdate()
    {
        currentSub.RunFixedUpdate();
    }

    private void LateUpdate()
    {
        currentSub.RunLateUpdate();
    }
}
