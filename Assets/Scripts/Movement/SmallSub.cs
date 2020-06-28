using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SmallSub : PlayerController
{
    public float mouseWheelInput;
    public override void RunUpdate()
    {
        //get input info
        _inputs = new Vector2(0f, Input.GetAxis("Vertical"));
        mouseWheelInput = Input.mouseScrollDelta.y;

        //get rotation info
        _lookCoOrds = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public override void RunFixedUpdate()
    {
        //sub movement        
        if (_inputs == Vector2.zero)
        {
            //slow down sub by -velocity
            if (rb.velocity != Vector3.zero)
            {
                rb.AddForce(-rb.velocity * speed);
            }
        }
        else
        {
            rb.AddForce((transform.forward * _inputs.y + transform.right * _inputs.x).normalized * speed);
            if (rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = rb.velocity.normalized * maxVelocity;
            }
        }

        //sub rotation
        if (_lookCoOrds != Vector2.zero)
        {
            _lookStorage += _lookCoOrds * Time.fixedDeltaTime * _lookSensitivity;
            _lookStorage.y = Mathf.Clamp(_lookStorage.y, -90f, 90f);

            Debug.Log(_lookStorage);
            transform.rotation = Quaternion.Euler(_lookStorage.y * -1, _lookStorage.x, 0.0f);
        }

        if(mouseWheelInput != 0f)
        {
            rb.AddForce(new Vector3(0f, mouseWheelInput, 0f));
            if (rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = rb.velocity.normalized * maxVelocity;
            }
        }
    }

    public override void RunLateUpdate()
    {
        
    }    

    public override void Walking(float speed, GameObject obj)
    {
        
    }
}
