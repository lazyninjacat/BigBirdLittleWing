using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SmallSub : PlayerController
{
    public float mouseWheelInput;
    private readonly int ballast = 20;
    Quaternion _rot;
    private bool isCon;
    [SerializeField] float _lerpSpeed;
    [SerializeField] Material[] _mat;





    public override void RunUpdate()
    {
        if (isCon != FindObjectOfType<BigSub>().isCon)
            isCon = FindObjectOfType<BigSub>().isCon;
        //get input info
        _inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        mouseWheelInput = Input.mouseScrollDelta.y;

        //get rotation info
        if (_inv) _lookCoOrds = (isCon) ? _lookCoOrds = new Vector2(-Input.GetAxis("Con X"), Input.GetAxis("Con Y")) : _lookCoOrds = new Vector2(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _lookCoOrds = (isCon) ? _lookCoOrds = new Vector2(Input.GetAxis("Con X"), Input.GetAxis("Con Y")) : _lookCoOrds = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseWheelInput = (isCon) ? mouseWheelInput = Input.GetAxis("Con Left Trig") + Input.GetAxis("Con Right Trig") : mouseWheelInput = Input.mouseScrollDelta.y;
        _lookSensitivity = (isCon) ? _lookSensitivity = 300 : _lookSensitivity = 20;

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

            _rot = Quaternion.Slerp(transform.rotation,
                                 Quaternion.Euler((_lookStorage.y * 4) * -1, _lookStorage.x * 3, 0f),
                                 _turnSpeed * Time.fixedDeltaTime);

            transform.rotation = RotationClamp(_rot);

        }

        if (mouseWheelInput != 0f)
        {
            rb.AddForce(new Vector3(0f, mouseWheelInput, 0f) * ballast);
            if (rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = rb.velocity.normalized * maxVelocity;
            }
        }
    }

    Quaternion RotationClamp(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
        //angleZ = Mathf.Clamp(angleZ, -15, 15);
        angleX = Mathf.Clamp(angleX, -60, 60);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
    public override void RunLateUpdate()
    {

    }
}
