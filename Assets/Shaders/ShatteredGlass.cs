using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShatteredGlass : MonoBehaviour
{
    [SerializeField] private float updateTime = 5f;
    private Material mat;
    private Timer timer;

    [SerializeField] AudioClip crackNoise;
    private AudioSource audioSource;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        timer = FindObjectOfType<Timer>();
        audioSource = GetComponent<AudioSource>();

        mat.SetFloat("_Control", 1f);
        StartCoroutine(Wait());
    }

    private void UpdatedShatter()
    {
        Debug.Log((timer.CurrentTime / timer.MaxTime) * 2 - 1);
        mat.SetFloat("_Control", (timer.CurrentTime / timer.MaxTime) * 2 - 1);
        audioSource.PlayOneShot(crackNoise);
    }

    IEnumerator Wait()
    {
        while(true)
        {
            yield return new WaitForSeconds(updateTime);
            UpdatedShatter();
        }
    }
}
