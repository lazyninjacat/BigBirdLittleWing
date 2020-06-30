using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] private string group = "groupName";
    public string GetGroup()
    {
        return group;
    }
    [SerializeField] private bool isActive = false;
    public bool GetIsActive()
    {
        return isActive;
    }
    [SerializeField] private LayerMask triggerLayer;

    //events
    public static event Action<string> activated;

    private void OnCollisionEnter(Collision collision)
    {
        if(triggerLayer == (triggerLayer | (1 << collision.gameObject.layer)))
        {
            isActive = true;
            activated?.Invoke(group);
            if (particles != null)
            {
                SetParticles();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (triggerLayer == (triggerLayer | (1 << collision.gameObject.layer)))
        {
            isActive = false;
            activated?.Invoke(group);
            if(particles != null)
            {
                SetParticles();
            }
        }
    }

    //bubbles
    private ParticlePlatform particles;

    private void SetParticles()
    {
        particles.SetParticles(!isActive);
    }

    private void Start()
    {
        particles = GetComponent<ParticlePlatform>();
        SetParticles();
    }

}
