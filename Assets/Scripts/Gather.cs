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
    [SerializeField] GameObject EnergyBarDeathAnimationLW;
    [SerializeField] GameObject EnergyBarDeathAnimationBB;
    [SerializeField] TextMeshProUGUI EnergyWarningText;
    [SerializeField] GameObject EnergyBarWarningAnimationLW;
    [SerializeField] GameObject EnergyBarWarningAnimationBB;
    [SerializeField] Docking docking;
    [SerializeField] GameObject LWParticles;
    [SerializeField] GameObject BBparticles;

    public int totalEnergyLW;
    public int totalEnergyBB;

    private bool isCountingBB;
    private bool isCountingLW;

    public bool LWisDead;
    public bool BBisDead;
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
        // Check conditions before draining energy from BB or LW
        if (!isCountingLW && totalEnergyLW > 0 && !docking.isDocked)
        {
            StartCoroutine(EnergyDrainLW());
        }
        if (!isCountingBB && totalEnergyBB > 0)
        {
            StartCoroutine(EnergyDrainBB());
        }             

        // Cap both BB and LW energy at 50
        if (totalEnergyLW > 50)
        {
            totalEnergyLW = 50;
        }
        if (totalEnergyBB > 50)
        {
            totalEnergyBB = 50;
        }

        // Check if LW is dead
        if (totalEnergyLW == 0)
        {
            LWisDead = true;
            EnergyBarDeathAnimationLW.SetActive(true);
        }
        else
        {
            EnergyBarDeathAnimationLW.SetActive(false);
            LWisDead = false;
        }

        // Check if BB is dead
        if (totalEnergyBB == 0)
        {
            BBisDead = true;
            EnergyBarDeathAnimationBB.SetActive(true);
            
        }
        else
        {
            BBisDead = false;
            EnergyBarDeathAnimationBB.SetActive(false);
        }

        // Play low energy warning animation is charge falls below 10
        if (totalEnergyBB < 10 && !BBisDead)
        {
            EnergyBarWarningAnimationBB.SetActive(true);
            EnergyBarWarningAnimationBB.GetComponent<Image>().fillAmount = EnergyBarBB.fillAmount;
        }
        else
        {
            EnergyBarWarningAnimationBB.SetActive(false);
        }

        if (totalEnergyLW < 10 && !LWisDead)
        {
            EnergyBarWarningAnimationLW.SetActive(true);
            EnergyBarWarningAnimationLW.GetComponent<Image>().fillAmount = EnergyBarLW.fillAmount;

        }
        else
        {
            EnergyBarWarningAnimationLW.SetActive(false);
        }


        // set UI text depending on which subs are dead
        if (BBisDead && LWisDead)
        {
            EnergyWarningText.text = "Game Over";
        }
        else if (BBisDead)
        {
            EnergyWarningText.text = "Big Bird has run out of energy. Recharge required.";
        }
        else if (LWisDead)
        {
            EnergyWarningText.text = "Little Wing has run out of energy.Recharge required.";
        }
        else
        {
            EnergyWarningText.text = "";
        }
    }


    private IEnumerator EnergyDrainLW()
    {
        isCountingLW = true;
        yield return new WaitForSeconds(5);
        totalEnergyLW--;
        UpdateEnergyBarLW();
        isCountingLW = false;
    }

    private IEnumerator EnergyDrainBB()
    {
        isCountingBB = true;
        yield return new WaitForSeconds(5);
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
            StartCoroutine(GatherHelperLW(energyAmount, collision.gameObject.GetComponent<ParticleSystem>()));
            collision.gameObject.GetComponent<Animation>().Stop("energyGeodeIdle");
            collision.gameObject.GetComponent<Animation>().Play("geodeGatherDim");
            activeGeodesList.Remove(collision.gameObject);
            inactiveGeodesList.Add(collision.gameObject);
        }      
    }


    private IEnumerator GatherHelperLW(int energyAmount, ParticleSystem particleSys)
    {
        LWParticles.SetActive(true);
        particleSys.Play();
        for (int i = energyAmount; i > 0; i--)
        {
            totalEnergyLW++;
            UpdateEnergyBarLW();
            yield return new WaitForSeconds(0.5f);
        }
        LWParticles.SetActive(false);
        particleSys.Stop();
    }

    public IEnumerator EnergyTransferHelper()
    {
        Debug.Log("Start Energy Transfer");
        isTransferingEnergy = true;
        LWParticles.SetActive(true);
        BBparticles.SetActive(true); ;
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
        LWParticles.SetActive(false);
        BBparticles.SetActive(false); ;
        docking.DockingPromptUI.GetComponent<TextMeshProUGUI>().text = "Energy transfer complete";
        isTransferingEnergy = false;
        docking.justFinishedEnergyTransfer = true;
    }
}
