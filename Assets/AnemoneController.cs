using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnemoneController : MonoBehaviour
{

    [SerializeField] AnemoneSway[] _anem;
    [SerializeField] Transform[] _targets;
    Vector3 _startPos;
    int i;
    // Start is called before the first frame update
    void Start()
    {
        _anem = FindObjectsOfType<AnemoneSway>();
        _startPos = transform.position;
        i = Random.Range(0, _targets.Length);
    }

    private void Update()
    {
       
        foreach (var item in _anem)
        {
            
           if(item._animate && Vector3.Distance(_targets[i].position,item.transform.position) > 1 && Vector3.Distance(_targets[i].position, item.transform.position) < 3)
                item.transform.position = Vector3.MoveTowards(item.transform.position, new Vector3(_targets[i].position.x,item.transform.position.y,_targets[i].position.z),Random.Range(1,2) * Time.deltaTime);
        }
    }
}
