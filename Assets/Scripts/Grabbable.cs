using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    PlayerManager _player;
    Gather _gather;
   [SerializeField] Material[] _mats;
    Sonar _sonarActive;
    public bool activate;
    // Start is called before the first frame update
    private void Start()
    {
        _sonarActive = FindObjectOfType<Sonar>();
        _player = FindObjectOfType<PlayerManager>();
        _gather = FindObjectOfType<Gather>();
    }
    private void Update()
    {
        if (activate)
        {
            if (Vector3.Distance(this.transform.position, _player.currentSub.transform.position) <= 50)
            {
                if (!_sonarActive.sonarIsReady)
                {
                    GetComponent<MeshRenderer>().material = _mats[1];
                    this.gameObject.layer = 31;
                }
            }        
        }
        if (_sonarActive.sonarIsReady)
        {
            GetComponent<MeshRenderer>().material = _mats[0];
            this.gameObject.layer = 28;
            activate = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            for (int i = 0; i < _gather.activeGeodesList.Count; i++)
            {
                if (_gather.activeGeodesList[i] == this.gameObject)
                {
                    activate = true;
                }
                else
                    this.gameObject.layer = 14;
            }
        }
    }
}
