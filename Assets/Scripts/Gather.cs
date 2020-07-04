using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Gather : MonoBehaviour
{
    
    [SerializeField] Image EnergyBarUI;

    public int totalEnergy;
    public bool isCounting;

    public List<GameObject> energyGeodesInSceneList;


    void Start()
    {
        energyGeodesInSceneList = new List<GameObject>();
        energyGeodesInSceneList = GameObject.FindGameObjectsWithTag("EnergyGeode").ToList();

        foreach (GameObject geode in energyGeodesInSceneList)
        {
            geode.GetComponent<Animation>().Play("energyGeodeIdle");
        }

        totalEnergy = 25;
        UpdateEnergyBarUI();
    }

    void Update()
    {

        if (!isCounting && totalEnergy > 0)
        {
            StartCoroutine(EnergyDrain());
        }
    }

    private IEnumerator EnergyDrain()
    {
        isCounting = true;
        yield return new WaitForSeconds(10);
        totalEnergy--;
        UpdateEnergyBarUI();
        isCounting = false;
    }

    private void UpdateEnergyBarUI()
    {
        if (totalEnergy <= 0)
        {
            EnergyBarUI.fillAmount = 0;
        }
        else if (totalEnergy < 50)
        {
            EnergyBarUI.fillAmount = ((float)totalEnergy / 50);
        }
        else if (totalEnergy >= 50)
        {
            EnergyBarUI.fillAmount = 1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (energyGeodesInSceneList.Contains(collision.gameObject))
        {
            Debug.Log("gathering started");

            int energyAmount = Int16.Parse(collision.gameObject.name);

            StartCoroutine(GatherHelper(energyAmount));

            collision.gameObject.GetComponent<Animation>().Stop("energyGeodeIdle");
            collision.gameObject.GetComponent<Animation>().Play("geodeGatherDim");
        }
    }

    private IEnumerator GatherHelper(int energyAmount)
    {
        for (int i = energyAmount; i > 0; i--)
        {
            totalEnergy++;
            UpdateEnergyBarUI();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (energyGeodesInSceneList.Contains(collision.gameObject))
        {
            Debug.Log("gathering ended");
        }
    }
}
