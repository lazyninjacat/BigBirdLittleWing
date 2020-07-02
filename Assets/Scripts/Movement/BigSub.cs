﻿using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSub : PlayerController
{
    /// <summary>
    /// TO DO: 
    /// 
    /// Done:
    /// Figure out why the arm moves strangely along the x axis when an object is grabbed ~Kyle
    /// </summary>
    //???
    [SerializeField] float _speedCur = 2;
    float _carryDistance;
    private readonly float _carrySpeed = 15f;
    bool _isInvert = false;

    //grabber
    [SerializeField] private LayerMask grabLayer;
    [SerializeField] private GameObject grabbedObject = null;
    [SerializeField] private float grabberSpeedH = 5f;
    [SerializeField] private float grabberSpeedV = 2f;
    [SerializeField] private float grabberDistanceMax = 8f;
    [SerializeField] private float grabberDistanceMin = 2f;
    public Vector3 grabObjDistance;
    [SerializeField] CCDIK _ik;
    [SerializeField] Transform _defaultIk;
    Vector3 _ref = Vector3.zero;
    [SerializeField] float _smoothing;
    //spotlight
    public GameObject _spotlight;
    [Tooltip("The velocity of the rigid body must be under this value for the spotlight to rotate")]
    public float maxSpotlightVelocity = 0.5f;

    protected override void Start()
    {
        base.Start();
        Cursor.lockState = CursorLockMode.Locked;
        _ik.solver.target = _defaultIk;
        currentState = state.MOVEEMPTY;
    }

    public void MouseInput()
    {
        //get rotation info
        _lookCoOrds = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }        
    public void KeyboardInput()
    {
        //get input info
        _inputs = new Vector2(0f, Input.GetAxis("Vertical"));
    }        
    public void Move()
    {
        //sub movement        
        if (_inputs.y == 0f)
        {
            //slow down sub by -velocity
            if (rb.velocity != Vector3.zero)
            {
                //rb.velocity = transform.forward * rb.velocity.magnitude;
                rb.AddForce(-rb.velocity * speed);
            }
        }
        else
        {            
            rb.AddForce((transform.forward * _inputs.y).normalized * speed);
            if (rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = rb.velocity.normalized * maxVelocity;
            }
        }
    }
    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }
    public void Spotlight()
    {
        if (_lookCoOrds != Vector2.zero)
        {            
            _lookStorage += _lookCoOrds * Time.deltaTime * _lookSensitivity;
            _spotlight.transform.rotation = Quaternion.Euler(_lookStorage.y * -1f, _lookStorage.x, 0.0f);
        }
    }
    public void SubRotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, 
                             Quaternion.Euler(-1f *_lookStorage.y, _lookStorage.x, 0f),
                             _turnSpeed * Time.fixedDeltaTime);
    }
    public void TryGrab()
    {
        //see if grab object
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("GState"))
        {           
            grabbedObject = GrabObject();
            if (grabbedObject != null)
            {
                //set as child
                grabbedObject.transform.parent = transform;
                grabObjDistance = grabbedObject.transform.localPosition;
                grabbedObject.transform.localRotation = Quaternion.identity;
                //set IK target to the grabbed object
                _ik.solver.target = grabbedObject.transform;
                //turn off gravity from rigidbody?
                grabbedObject.GetComponent<Rigidbody>().useGravity = false;
                Cursor.lockState = CursorLockMode.Locked;
                currentState = state.MOVEGRAB;
            }
            else
                //reset Ik target
                _ik.solver.target = _defaultIk;
        }
    }
    public void ReleaseGrab()
    {
        //turn on gravity from rigidbody?
        grabbedObject.transform.parent = null;
        grabbedObject.GetComponent<Rigidbody>().useGravity = true;
        grabbedObject = null;
        Cursor.lockState = CursorLockMode.Locked;
        currentState = state.MOVEEMPTY;
        //reset Ik target
        _ik.solver.target = _defaultIk;
        //close arm animator
        anim.SetBool("Arm", false);
    }
    public void MoveGrab()
    {
        //forward back
        if (Input.mouseScrollDelta.y != 0f)
        {
            //move distance to from 
            if(grabObjDistance.z + (Input.mouseScrollDelta.y * grabberSpeedV * Time.fixedDeltaTime) < grabberDistanceMax &&
               grabObjDistance.z + (Input.mouseScrollDelta.y * grabberSpeedV * Time.fixedDeltaTime) > grabberDistanceMin)
            {
                grabObjDistance += new Vector3(0f, 0f, Input.mouseScrollDelta.y * grabberSpeedV * Time.fixedDeltaTime);
            }
        }

        if(_lookCoOrds != Vector2.zero)
        {
            grabObjDistance += ((Vector3.right * _lookCoOrds.x) + (Vector3.up * _lookCoOrds.y)) * grabberSpeedH * Time.fixedDeltaTime;
        }
    }
    public void UpdateGrab()
    {
        grabbedObject.transform.localPosition = grabObjDistance;
        grabbedObject.transform.localRotation = Quaternion.identity;
    }

    public enum state { NONE, MOVEEMPTY, MOVEWITHGRAB, TRYGRAB, MOVEGRAB }
    public state currentState = state.NONE;

    public override void RunUpdate()
    {
        switch (currentState)
        {
            case state.MOVEEMPTY:
                MouseInput();
                KeyboardInput();
                if (Input.GetMouseButtonUp(1))
                {
                    currentState = state.TRYGRAB;
                    anim.SetBool("Arm", true);
                    Stop();
                    Cursor.lockState = CursorLockMode.Confined;
                }
                break;
            case state.MOVEWITHGRAB:
                MouseInput();
                KeyboardInput();
                if (Input.GetMouseButtonUp(0))
                {
                    ReleaseGrab();
                }
                if (Input.GetMouseButtonUp(1))
                {
                    Stop();
                    currentState = state.MOVEGRAB;
                }
                break;
            case state.TRYGRAB:
                MouseInput();
                Spotlight();
                if(Input.GetMouseButtonUp(0))
                {
                    TryGrab();
                }
                if(Input.GetMouseButtonUp(1))
                {
                    currentState = state.MOVEEMPTY;
                    anim.SetBool("Arm", false);

                    Cursor.lockState = CursorLockMode.Locked;
                }                
                break;
            case state.MOVEGRAB:
                MouseInput();
                if(Input.GetMouseButtonUp(0))
                {
                    ReleaseGrab();
                }
                if(Input.GetMouseButtonUp(1))
                {
                    currentState = state.MOVEWITHGRAB;
                }
                break;
            default:
                break;
        }
    }

    public override void RunFixedUpdate()
    {
        switch (currentState)
        {
            case state.MOVEEMPTY:
                Move();
                //if(rb.velocity.magnitude < maxSpotlightVelocity)
                //{
                    Spotlight();
               // }
                SubRotate();
                break;
            case state.MOVEWITHGRAB:
                Move();
                //if (rb.velocity.magnitude < maxSpotlightVelocity)
                //{
                    Spotlight();
                //}
                SubRotate();
                UpdateGrab();
                break;
            case state.TRYGRAB:
                if (_lookCoOrds != Vector2.zero)
                {
                    _defaultIk.transform.localPosition += ((Vector3.right * _lookCoOrds.x) + (Vector3.up * _lookCoOrds.y)) * grabberSpeedH * Time.fixedDeltaTime;
                }
                break;
            case state.MOVEGRAB:
                MoveGrab();
                UpdateGrab();
                SubRotate();
                break;
            default:
                break;
        }
    }


    public override void RunLateUpdate()
    {
        //if(!_isInvert)
        //{
        //    _spot.transform.localRotation = Quaternion.Euler(_lookStorage.y * -1, _lookStorage.x, 0.0f);
        //    _subOne.transform.localRotation = Quaternion.Slerp(_subOne.transform.rotation, Quaternion.Euler(0.0f, _lookStorage.x, 0.0f), _turnSpeed * Time.deltaTime);
        //}
        //else
        //{
        //    _spot.transform.localRotation = Quaternion.Euler(_lookStorage.y * 1, _lookStorage.x, 0.0f);
        //    _subOne.transform.localRotation = Quaternion.Slerp(_subOne.transform.rotation, Quaternion.Euler(0.0f, _lookStorage.x, 0.0f), _turnSpeed * Time.deltaTime);
        //}
    }
    

    private GameObject GrabObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //check for geod layer
            if (grabLayer == (grabLayer | (1 << hit.collider.gameObject.layer)))
            {
                if (Vector3.Distance(hit.collider.transform.position, transform.position) < grabberDistanceMax &&
                Vector3.Distance(hit.collider.transform.position, transform.position) > grabberDistanceMin)
                {
                    Debug.Log("Grabbed Object: " + hit.collider.gameObject.name);
                    return hit.collider.gameObject;
                }
            }
        }

        return null;
    }

    public override void Walking(float speed, GameObject obj)
    {
        
    }
}


