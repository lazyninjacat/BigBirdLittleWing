using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Sonar : MonoBehaviour
{
    public bool sonarIsReady;
    public int sonarCooldownTimer;
    PlayerManager _player;
    [SerializeField] GameObject sonarCharge;
    [SerializeField] GameObject sonarSphere;
    [SerializeField] Camera _sonarCam;
    
    // Start is called before the first frame update
    void Start()
    {
        sonarIsReady = true;
        sonarSphere.SetActive(false);
        StartCoroutine(SonarCountdown());
        _player = FindObjectOfType<PlayerManager>();
        _sonarCam.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _player.currentSub.transform.position;


        if (Input.GetButtonDown("Radar"))
        {
            if (sonarIsReady)
            {
                sonarIsReady = false;
                sonarSphere.SetActive(true);
                _sonarCam.gameObject.SetActive(true);
                StartCoroutine(SonarCountdown());
            }
            else
            {
                return;
             //   Debug.Log("Sonar not ready yet");
            }
        }
    }

    private IEnumerator SonarCountdown()
    {
        sonarCharge.GetComponent<Animation>().Play();
       // Debug.Log("Sonar cooldown start");
        for (int i = 0; i < sonarCooldownTimer - 1; i++)
        {
           // Debug.Log("Cooldown Timer: " + ((sonarCooldownTimer-1) - i));

            yield return new WaitForSeconds(1);

        }
        sonarIsReady = true;
        _sonarCam.gameObject.SetActive(false);

      //  Debug.Log("Sonar is Ready");
        sonarSphere.SetActive(false);
    }

}
