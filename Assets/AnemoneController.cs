using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnemoneController : MonoBehaviour
{

    [SerializeField] AnemoneSway[] _anem;
    // Start is called before the first frame update
    void Start()
    {
        _anem = FindObjectsOfType<AnemoneSway>();
    }

    private void Update()
    {
        foreach (var item in _anem)
        {
            if(Vector3.Distance(item.transform.position,transform.position) <= 3 && !item._ishit)
            item.transform.position = Vector3.MoveTowards(item.transform.position, transform.position - item.transform.position, 1 * Time.deltaTime);
        }
    }
}
