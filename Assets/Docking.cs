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
    [SerializeField] GameObject DockingPromptUI;
    [SerializeField] GameObject LW_Cam;

    private bool isOnDollyCart;
    private bool triggeredDockingCollider;
    private bool isStart;
    private bool justFinishedEnergyTransfer;

    private Vector3 DockPosition;



    private void Start()
    {
        isStart = true;
        gameObject.GetComponent<CinemachineDollyCart>().enabled = false;
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
                    if (Input.GetButtonDown("Grab"))
                    {
                        StartCoroutine(DockingHelper());
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

        //Set the first waypoint of the dolly track to the little sub's current position.
        // DockingDollyPath.m_Waypoints[0].position = gameObject.transform.position;

        //Set the little sub to position 0
        gameObject.GetComponent<CinemachineDollyCart>().m_Position = 0;

        // Wait a second then begin energy transfer
        yield return new WaitForSeconds(1);
        StartCoroutine(gather.EnergyTransferHelper());

        // Wait 3 seconds for energy to transfer
        yield return new WaitForSeconds(3);

        // Change DockingUI text, and reset bools
        DockingPromptUI.SetActive(true);
        justFinishedEnergyTransfer = true;
        isOnDollyCart = false;        
        DockingPromptUI.GetComponent<TextMeshProUGUI>().text = "Energy transfer complete";

        //Wait two second then clear the docking UI and reset justfinishedenergytransfer bool to false
        yield return new WaitForSeconds(2);
        DockingPromptUI.SetActive(false);
        justFinishedEnergyTransfer = false;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (LW_Cam.activeSelf)
        {
            if (other.gameObject.tag == "DockingCollider")
            {
                Debug.Log("in docking collider");
                DockingPromptUI.SetActive(true);
                if (!justFinishedEnergyTransfer)
                {
                    DockingPromptUI.GetComponent<TextMeshProUGUI>().text = "press 'A' to transfer energy";
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
