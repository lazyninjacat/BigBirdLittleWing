using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_Sequence : MonoBehaviour
{
    [SerializeField] GameObject Text1;
    [SerializeField] GameObject Text2;
    [SerializeField] GameObject Text3;
    [SerializeField] GameObject Text4;
    [SerializeField] GameObject Text5;
    [SerializeField] GameObject Text6;
    [SerializeField] GameObject SkipText;
    [SerializeField] GameObject PlayerCam;
    [SerializeField] GameObject SurfaceCam;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject dollyCart;

    private bool isStartSequence;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCam.SetActive(false);
        SurfaceCam.SetActive(true);
        //BackgroundPanel.SetActive(true);
        StartCoroutine(StartSequence());
    }

    private void Update()
    {
        if (isStartSequence)
        {
            Player.transform.position = dollyCart.transform.position;
            PlayerCam.SetActive(false);
            SurfaceCam.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StopAllCoroutines();
            StartCoroutine(SkipStart());

        }
    }

    private IEnumerator SkipStart()
    {
        dollyCart.GetComponent<CinemachineDollyCart>().m_Position = 1978f;
        yield return new WaitForSeconds(1);

        isStartSequence = false;
        SurfaceCam.SetActive(false);
        PlayerCam.SetActive(true);


        Text1.SetActive(false);
        Text2.SetActive(false);
        Text3.SetActive(false);
        Text4.SetActive(false);
        Text5.SetActive(false);
        Text6.SetActive(false);
        SkipText.SetActive(false);
    }

    private IEnumerator StartSequence()
    {
        PlayerCam.SetActive(false);
        SurfaceCam.SetActive(true);

        isStartSequence = true;
        dollyCart.GetComponent<CinemachineDollyCart>().m_Position = 0;

        Text1.SetActive(true);
        yield return new WaitUntil(() => !Text1.GetComponent<Animation>().isPlaying);
        Text1.SetActive(false);
        Text2.SetActive(true);
        yield return new WaitUntil(() => !Text2.GetComponent<Animation>().isPlaying);
        Text2.SetActive(false);
        Text3.SetActive(true);
        yield return new WaitUntil(() => !Text3.GetComponent<Animation>().isPlaying);
        Text3.SetActive(false);
        Text4.SetActive(true);
        yield return new WaitUntil(() => !Text4.GetComponent<Animation>().isPlaying);
        Text4.SetActive(false);
        Text5.SetActive(true);
        yield return new WaitUntil(() => !Text5.GetComponent<Animation>().isPlaying);
        Text5.SetActive(false);
        Text6.SetActive(true);
        yield return new WaitUntil(() => !Text6.GetComponent<Animation>().isPlaying);
        Text6.SetActive(false);
        yield return new WaitUntil(() => dollyCart.GetComponent<CinemachineDollyCart>().m_Position == 1978f);

        isStartSequence = false;
        PlayerCam.SetActive(true);
    }


}
