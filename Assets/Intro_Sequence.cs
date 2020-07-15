using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_Sequence : MonoBehaviour
{
    [SerializeField] GameObject BackgroundPanel;
    [SerializeField] GameObject Text1;
    [SerializeField] GameObject Text2;
    [SerializeField] GameObject Text3;
    [SerializeField] GameObject Text4;
    [SerializeField] GameObject Text5;
    [SerializeField] GameObject Text6;
    [SerializeField] GameObject SkipText;
    [SerializeField] GameObject PlayerCam;
    [SerializeField] GameObject SurfaceCam;

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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StopAllCoroutines();
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
    }

    private IEnumerator StartSequence()
    {
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

        //BackgroundPanel.GetComponent<Animation>().Play();
        //yield return new WaitUntil(() => !BackgroundPanel.GetComponent<Animation>().isPlaying);

        PlayerCam.SetActive(true);
    }


}
