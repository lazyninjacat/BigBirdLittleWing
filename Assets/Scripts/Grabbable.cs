using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    PlayerManager _player;
   [SerializeField] Material[] _mats;
    Sonar _sonarActive;
    public bool activate;
    // Start is called before the first frame update
    private void Start()
    {
        _sonarActive = FindObjectOfType<Sonar>();
        _player = FindObjectOfType<PlayerManager>();
    }
    private void Update()
    {
        if (activate)
        {
            if (Vector3.Distance(this.transform.position, _player.currentSub.transform.position) <= 50)
            {
                if (!_sonarActive.sonarIsReady && _player.currentSub == _player.players[0])
                {
                    GetComponent<MeshRenderer>().material = _mats[1];
                    this.gameObject.layer = 31;
                }
                //else if(!_sonarActive.sonarIsReady && _player.currentSub == _player.players[1])
                //{
                //    if (this.gameObject.tag == "EnergyGeode")
                //    {
                //        print(this.gameObject.name.ToString());
                //        GetComponent<MeshRenderer>().material = _mats[1];
                //        this.gameObject.layer = 31;
                //    }
                //}
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
        if (other.gameObject.layer == 13) activate = true;
    }
}
