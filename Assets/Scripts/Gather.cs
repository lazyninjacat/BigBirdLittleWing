using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gather : MonoBehaviour
{
    
    [SerializeField] Image EnergyBarLW;
    [SerializeField] Image EnergyBarBB;
    [SerializeField] Docking docking;


    public int totalEnergyLW;
    public int totalEnergyBB;

    private bool isCountingBB;
    private bool isCountingLW;
    public bool isTransferingEnergy;




    public List<GameObject> activeGeodesList;
    public List<GameObject> inactiveGeodesList;

    void Start()
    {
        activeGeodesList = new List<GameObject>();
        inactiveGeodesList = new List<GameObject>();
        activeGeodesList = GameObject.FindGameObjectsWithTag("EnergyGeode").ToList();

        foreach (GameObject geode in activeGeodesList)
        {
            geode.GetComponent<Animation>().Play("energyGeodesIdle");
        }

        totalEnergyLW = 25;
        UpdateEnergyBarLW();

        totalEnergyBB = 25;
        UpdateEnergyBarBB();
    }

    void Update()
    {

        if (!isCountingLW && totalEnergyLW > 0 && docking.isDocked == false)
        {
            StartCoroutine(EnergyDrainLW());
        }

        if (!isCountingBB && totalEnergyBB > 0)
        {
            StartCoroutine(EnergyDrainBB());
        }

    
        
    }


    private IEnumerator EnergyDrainLW()
    {
        isCountingLW = true;
        yield return new WaitForSeconds(10);
        totalEnergyLW--;
        UpdateEnergyBarLW();
        isCountingLW = false;
    }

    private IEnumerator EnergyDrainBB()
    {
        isCountingBB = true;
        yield return new WaitForSeconds(10);
        totalEnergyBB--;
        UpdateEnergyBarBB();
        isCountingBB = false;
    }

    private void UpdateEnergyBarLW()
    {
        if (totalEnergyLW <= 0)
        {
            EnergyBarLW.fillAmount = 0;
        }
        else if (totalEnergyLW < 50)
        {
            EnergyBarLW.fillAmount = ((float)totalEnergyLW / 50);
        }
        else if (totalEnergyLW >= 50)
        {
            EnergyBarLW.fillAmount = 1;
        }
    }

    private void UpdateEnergyBarBB()
    {
        if (totalEnergyBB <= 0)
        {
            EnergyBarBB.fillAmount = 0;
        }
        else if (totalEnergyBB < 50)
        {
            EnergyBarBB.fillAmount = ((float)totalEnergyBB / 50);
        }
        else if (totalEnergyBB >= 50)
        {
            EnergyBarBB.fillAmount = 1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (activeGeodesList.Contains(collision.gameObject))
        {
            Debug.Log("gathering started");

            int energyAmount = Int16.Parse(collision.gameObject.name);

            StartCoroutine(GatherHelperLW(energyAmount));

            collision.gameObject.GetComponent<Animation>().Stop("energyGeodeIdle");
            collision.gameObject.GetComponent<Animation>().Play("geodeGatherDim");

            activeGeodesList.Remove(collision.gameObject);
            inactiveGeodesList.Add(collision.gameObject);
        }
       
       
    }


    private IEnumerator GatherHelperLW(int energyAmount)
    {
        for (int i = energyAmount; i > 0; i--)
        {
            totalEnergyLW++;
            UpdateEnergyBarLW();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator EnergyTransferHelper()
    {
        Debug.Log("Start Energy Transfer");
        isTransferingEnergy = true;

        for (int i = 1; i < totalEnergyLW; i++)
        {
            if (totalEnergyBB < 50 && totalEnergyLW > 10)
            {
                totalEnergyLW--;
                totalEnergyBB++;
                UpdateEnergyBarLW();
                UpdateEnergyBarBB();
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                break;
            }
       
        }
        

        docking.DockingPromptUI.GetComponent<TextMeshProUGUI>().text = "Energy transfer complete";

        isTransferingEnergy = false;
        docking.justFinishedEnergyTransfer = true;

    }
}
