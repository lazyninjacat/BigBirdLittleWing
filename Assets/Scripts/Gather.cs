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


    // Start is called before the first frame update
    void Start()
    {
        totalEnergy = 25;
        UpdateEnergyBarUI();
    }

    // Update is called once per frame
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
        if (collision.gameObject.tag == "EnergyGeode");
        {
            Debug.Log("gathering started");
            totalEnergy++;
            UpdateEnergyBarUI();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "EnergyGeode") ;
        {
            Debug.Log("gathering ended");
        }
    }

}
