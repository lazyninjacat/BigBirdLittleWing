using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticlePlatform : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;

    private void Start()
    {
        particles = Instantiate(particles, this.transform);
        particles.transform.localPosition = Vector3.zero;
        particles.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
        particles.Stop();
    }
    public void SetParticles(bool b)
    {
        if(b)
        {
            particles.Play();
        }
        else
        {
            particles.Stop();
        }
    }
}
