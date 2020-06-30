using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class Timer : MonoBehaviour
{
    [Range(0, 59)] [SerializeField] private int Minutes = 59;
    [Range(0, 59)] [SerializeField] private int Seconds = 59;
    private TextMeshProUGUI text;
    public float MaxTime { get; private set; }
    public float CurrentTime { get; private set; }
    public bool IsActive { get; private set; }

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        MaxTime = Minutes * 60 + Seconds;
        CurrentTime = MaxTime;

        IsActive = true;
    }

    private void FixedUpdate()
    {
        if(IsActive)
        {
            CurrentTime -= Time.fixedDeltaTime;
            UpdateText();
        }
    }

    private void UpdateText()
    {
        int min = (int)CurrentTime / 60;
        int sec = (int)CurrentTime % 60;
        if(sec < 10)
        {
            text.text = min + ":0" + sec; 
        }
        else
        {
            text.text = min + ":" + sec; 
        }
    }
}
