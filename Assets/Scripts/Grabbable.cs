using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{

   [SerializeField] Material[] _mats;
    Sonar _sonarActive;
    // Start is called before the first frame update
    private void Start()
    {
        _sonarActive = FindObjectOfType<Sonar>();
    }
    private void Update()
    {
        if (!_sonarActive.sonarIsReady) GetComponent<MeshRenderer>().material = _mats[1];
        else
            GetComponent<MeshRenderer>().material = _mats[0];

    }
}
