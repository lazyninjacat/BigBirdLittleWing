using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Docking : MonoBehaviour
{
    private Vector3 DockPosition;

    [SerializeField] CinemachineSmoothPath DockingDollyPath;
    [SerializeField] Transform Dock;
    [SerializeField] Transform DollyCart;
    [SerializeField] GameObject DockingCollider;
    [SerializeField] Gather gather;
    [SerializeField] GameObject DockingPromptUI;

    private bool isOnDollyCart;
    private bool isInsideDockingCollider;
    private bool isStart;

    private void Start()
    {
        isStart = true;
    }

    void Update()
    {
        DockPosition = new Vector3(Dock.position.x, Dock.position.y, Dock.position.z);

        if (!isStart)
        {
            if (isInsideDockingCollider)
            {
                if (Input.GetButtonDown("Grab"))
                {                  
                    DockingHelper();
                }
            }      
        }     
        else
        {
            DockingPromptUI.SetActive(false);
        }
    }

    private IEnumerator DockingHelper()
    {
        Debug.Log("Docking Helper");
        gameObject.GetComponent<SmallSub>().enabled = false;
        gameObject.transform.position = DollyCart.position;
        DockingPromptUI.SetActive(false);
        isInsideDockingCollider = false;
        DockingDollyPath.m_Waypoints[1].position = Dock.position;
        DockingDollyPath.m_Waypoints[0].position = gameObject.transform.position;
        isOnDollyCart = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitUntil(() => gameObject.transform.position == DockPosition);
        StartCoroutine(gather.EnergyTransferHelper());
        yield return new WaitUntil(() => gather.isTransferingEnergy = false);
        isOnDollyCart = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        DockingPromptUI.SetActive(true);
        DockingPromptUI.GetComponent<TextMeshProUGUI>().text = "Energy transfer complete";
        gameObject.GetComponent<SmallSub>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DockingCollider")
        {
            Debug.Log("in docking collider");
            DockingPromptUI.SetActive(true);
            DockingPromptUI.GetComponent<TextMeshProUGUI>().text = "press 'A' to transfer energy";
            isInsideDockingCollider = true;
        }     
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "DockingCollider")
        {
            if (!isStart)
            {
                isInsideDockingCollider = false;
                DockingPromptUI.SetActive(false);
            }
            else
            {
                isStart = false;
            }
            Debug.Log("exit docking collider");
        }
    }
}
