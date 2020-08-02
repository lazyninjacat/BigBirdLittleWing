using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Docking : MonoBehaviour
{
    [SerializeField] CinemachineSmoothPath DockingDollyPath;
    [SerializeField] Transform Dock;
    [SerializeField] GameObject DockingCollider;
    [SerializeField] Gather gather;
    [SerializeField] public GameObject DockingPromptUI;
    [SerializeField] GameObject LW_Cam;
        
    public bool justFinishedEnergyTransfer;
    public bool isDocked;
    public bool isUndocking;

    private bool isOnDollyCart;
    private bool triggeredDockingCollider;
    private bool isStart;

    private Vector3 DockPosition;

    private void Start()
    {
        isStart = true;
        gameObject.GetComponent<CinemachineDollyCart>().enabled = false;
        StartCoroutine(DockingHelper());
    }

    void Update()
    {
        DockPosition = new Vector3(Dock.position.x, Dock.position.y, Dock.position.z);

        if (LW_Cam.activeSelf)
        {
            if (isStart == false)
            {
                if (triggeredDockingCollider)
                {
                    if (!isDocked)
                    {
                        if (Input.GetButtonDown("Grab"))
                        {
                            StartCoroutine(DockingHelper());
                        }
                    }                  
                }
            }
            else
            {
                DockingPromptUI.SetActive(false);
            }

            if (isOnDollyCart)
            {
                gameObject.GetComponent<SmallSub>().enabled = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<CinemachineDollyCart>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<CinemachineDollyCart>().enabled = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<SmallSub>().enabled = true;

            }
        }

        if (isDocked)
        {
            gameObject.transform.position = DockPosition;
            gameObject.GetComponent<SmallSub>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (isUndocking)
        {
            isDocked = false;
            isOnDollyCart = true;

            StartCoroutine(UndockingHelper());
        }

        if (isDocked && LW_Cam.activeSelf)
        {
            DockingPromptUI.GetComponent<TextMeshProUGUI>().text = "Docked. Press 'X' (or keyboard 'T') to begin energy transfer, press 'Y' (or keyboard 'U') to undock, press 'B' to switch to bigsub";

            if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("x button");
                StartCoroutine(gather.EnergyTransferHelper());
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.U))
            {
                Debug.Log("y button");
                isUndocking = true;
                DockingPromptUI.GetComponent<TextMeshProUGUI>().text = "Undocked";
            }
        }
        else if (LW_Cam.activeSelf == false)
        {
            DockingPromptUI.GetComponent<TextMeshProUGUI>().text = "";
        }


        //TODO replace this with an invisable boundary collider sphere gameobject

        //if(Vector3.Distance(transform.position,Dock.position) >= 100) StartCoroutine(DockingHelper());
    }

    private IEnumerator UndockingHelper()
    {
        gameObject.GetComponent<Animation>().Play("undock");
        yield return new WaitForSeconds(1);
        isUndocking = false;
        isOnDollyCart = false;
    }

    private IEnumerator DockingHelper()
    {
        Debug.Log("Docking Helper");

        //reset trigger
        triggeredDockingCollider = false;        

        //turn off the UI prompt
        DockingPromptUI.SetActive(false);

        //Turn on the Dolly Cart
        isOnDollyCart = true;

        //Set the little sub to position 0
        gameObject.GetComponent<Animation>().Play("docking");

        // wait for dolly to complete track to dock position then set isDocked to true
        yield return new WaitForSeconds(1);
        isDocked = true;
    }



    private void OnTriggerEnter(Collider other)
    {
        // Chck that player is currently on the LW sub by checking the active vcam
        if (LW_Cam.activeSelf)
        {
            // Check that the collider is the docking collider
            if (other.gameObject.tag == "DockingCollider")
            {
                Debug.Log("in docking collider");
                DockingPromptUI.SetActive(true);
                if (!justFinishedEnergyTransfer)
                {
                    DockingPromptUI.GetComponent<TextMeshProUGUI>().text = "press 'A' on gamepad, or click LMB to dock";
                }
                triggeredDockingCollider = true;
            }
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        if (LW_Cam.activeSelf)
        {
            if (other.gameObject.tag == "DockingCollider")
            {
                if (!isStart)
                {
                    //justFinishedEnergyTransfer = false;
                    triggeredDockingCollider = false;
                    DockingPromptUI.SetActive(false);
                    justFinishedEnergyTransfer = false;
                }
                else
                {
                    Debug.Log("Start now off");
                    isStart = false;
                }
                Debug.Log("exit docking collider");
            }
        }      
    }
}
