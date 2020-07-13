using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    PlayerManager _player;
   [SerializeField] Material[] _mats;
    Sonar _sonarActive;
    // Start is called before the first frame update
    private void Start()
    {
        _sonarActive = FindObjectOfType<Sonar>();
        _player = FindObjectOfType<PlayerManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) print(Vector3.Distance(this.transform.position, _player.currentSub.transform.position));
        if (Vector3.Distance(this.transform.position, _player.currentSub.transform.position) <= 50)
        {
            if (!_sonarActive.sonarIsReady)
            {
                GetComponent<MeshRenderer>().material = _mats[1];
                this.gameObject.layer = 31;
            }      
        }
        if(_sonarActive.sonarIsReady)
            {
                GetComponent<MeshRenderer>().material = _mats[0];
                this.gameObject.layer = 28;
            }
    }
}
