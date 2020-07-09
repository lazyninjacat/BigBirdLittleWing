using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class AnemoneSway : MonoBehaviour
{

    SphereCollider _col;
    Rigidbody _rb;
    [SerializeField]public Vector3 _idle;
    [SerializeField] Vector3 _hitPos;
    float _lerp = 1f;
    bool _return;
    public bool _ishit;
    // Start is called before the first frame update
    void Start()
    {
        _col = GetComponent<SphereCollider>();
        _idle = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_ishit)
        {
            if (Vector3.Distance(transform.position, _idle) >= 0.011) _return = true;
            if (Vector3.Distance(transform.position, _idle) <= 0.00) _return = false;

            if (_return)
            {
                transform.position = Vector3.MoveTowards(transform.position, _idle, _lerp * Time.deltaTime);
                _col.center = Vector3.zero;
            }
        }
    }
    private void FixedUpdate()
    {

        if (!_return)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        _ishit = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        _ishit = false;
    }
}
