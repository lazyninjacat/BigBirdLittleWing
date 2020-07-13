using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BigSub : PlayerController
{
    /// <summary>
    /// TO DO: 
    /// Animate the arm extending to grab object
    /// Animate claws opening/closing to grab
    ///
    /// Done:
    /// Refactor controller support into the playerManager ~ Kyle
    /// Finish controller support for big and little subs ~ Kyle
    /// Arm pickup/drop working **no errors or weird behaviors**
    /// Clamp Arm Rotation ~ Kyle
    /// Clamp Rotation of _spotlight on x and y rotations ~ Kyle
    /// Figure out why the arm moves strangely along the x axis when an object is grabbed ~Kyle
    /// </summary>
   

    /////////////////////////////////////////
    /// EnegyCharge stuff
    [SerializeField] GameObject EnergyChargeBarUI;
    public int totalEnergy;
    public bool isCharged;
    /////////////////////////////////////////
    /////////////////////////////////////////
    
    //grabber
    [SerializeField] private LayerMask grabLayer;
    private GameObject grabbedObject = null;
    private float grabberSpeedH = 5f;
    public Vector3 grabObjDistance;
    [SerializeField] CCDIK _ik;
    [SerializeField] Transform _defaultIk;
    [SerializeField] GameObject _defaulIKRestPos;
    [SerializeField] float _smoothing = 0.7f;
    float _distanceFromPickup = Mathf.Infinity;
    GameObject closest = null;

    //spotlight
    public GameObject _spotlight;
    [Tooltip("The velocity of the rigid body must be under this value for the spotlight to rotate")]
    public float maxSpotlightVelocity = 0.5f;

    /// Move sub up/down
    public float mouseWheelInput;
    private readonly int ballast = 20;

    //Controller Support
    public bool isCon;

    protected override void Start()
    {
        base.Start();
        Cursor.lockState = CursorLockMode.Locked;
        _ik.solver.target = _defaultIk;
        currentState = state.MOVEEMPTY;
        _defaulIKRestPos.transform.position = _defaultIk.position;
    }
    
    public enum state { NONE, MOVEEMPTY, MOVEWITHGRAB, TRYGRAB, MOVEGRAB }
    public state currentState = state.NONE;

    public override void RunUpdate()
    {
        grabberSpeedH = (isCon) ? grabberSpeedH = 50 : grabberSpeedH = 5;
        _inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (_inv) _lookCoOrds = (isCon) ? _lookCoOrds = new Vector2(-Input.GetAxis("Con X"), Input.GetAxis("Con Y")) : _lookCoOrds = new Vector2(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _lookCoOrds = (isCon) ? _lookCoOrds = new Vector2(Input.GetAxis("Con X"), Input.GetAxis("Con Y")) : _lookCoOrds = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseWheelInput = (isCon) ? mouseWheelInput = Input.GetAxis("Con Left Trig") + Input.GetAxis("Con Right Trig") : mouseWheelInput = Input.mouseScrollDelta.y;
        _lookSensitivity = (isCon) ? _lookSensitivity = 300 : _lookSensitivity = 20;
      
        /////Temp Controller Switch
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isCon) isCon = false;
            else isCon = true;
        }

        /////////////////////////////////////////
        /// EnegyCharge stuff

        if (totalEnergy > 0)
        {
            isCharged = true;
        }
        else
        {
            isCharged = false;
        }
        //////////////////////////////////////////

        if (isCharged)
        {
            switch (currentState)
            {
                case state.MOVEEMPTY:
                    if (Input.GetButtonUp("Arm"))
                    {
                        currentState = state.TRYGRAB;
                        AudioManager.a_Instance.PlayOneShotByName("ArmOpen");
                        anim.SetBool("Arm", true);
                        Stop();
                    }
                    break;
                case state.MOVEWITHGRAB:
                    if (Input.GetButtonUp("Grab"))
                    {
                        ReleaseGrab();
                        currentState = state.MOVEEMPTY;
                    }
                    if (Input.GetButtonUp("Arm"))
                    {
                        Stop();
                        currentState = state.MOVEGRAB;
                    }
                    break;
                case state.TRYGRAB:
                    if (Input.GetButtonUp("Grab"))
                    {
                        TryGrab();
                    }
                    if (Input.GetButtonUp("Arm"))
                    {
                        currentState = state.MOVEEMPTY;
                        anim.SetBool("Arm", false);
                    }
                    break;
                case state.MOVEGRAB:
                    if (Input.GetButtonUp("Grab"))
                    {
                        ReleaseGrab();
                    }
                    if (Input.GetButtonUp("Arm"))
                    {
                        currentState = state.MOVEWITHGRAB;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("Energy is empty! Need to re-charge in order to operate big sub");
        }

    }

    public override void RunFixedUpdate()
    {
        
        /////////////////////////////////////////
        /// EnegyCharge stuff
        if (totalEnergy > 0)
        {
            isCharged = true;
        }
        else
        {
            isCharged = false;
        }
        //////////////////////////////////////////
        //////////////////////////////////////////



        if (isCharged)
        {
            switch (currentState)
            {
                case state.MOVEEMPTY:
                    Move(speed);
                    //if(rb.velocity.magnitude < maxSpotlightVelocity)
                    //{
                    Spotlight();
                    // }
                    SubRotate();
                    break;
                case state.MOVEWITHGRAB:
                    Move(speed - 1);
                    Spotlight();
                    SubRotate();
                    UpdateGrab();
                    break;
                case state.TRYGRAB:
                    //SubRotate();
                    Spotlight();
                    Move(speed-1);
                    if (_lookCoOrds != Vector2.zero)
                    {
                        if(Vector3.Distance(_defaultIk.position,_defaulIKRestPos.transform.position) < 2) 
                             _defaultIk.transform.localPosition += ((Vector3.right * _lookCoOrds.x) + (Vector3.up * _lookCoOrds.y)) * grabberSpeedH * Time.fixedDeltaTime;
                         else
                            _defaultIk.transform.localPosition = Vector3.Slerp(_defaultIk.transform.localPosition, _defaulIKRestPos.transform.localPosition, _smoothing * Time.deltaTime);
                    }
                    break;
                case state.MOVEGRAB:
                    Move(speed - 1);
                    Spotlight();
                    MoveGrab();
                    UpdateGrab();
                    break;
                default:
                    break;
            }
           
        }
        else
        {
            Debug.Log("Energy is empty! Need to re-charge in order to operate big sub");
        }

    }


    public override void RunLateUpdate()
    {
    }

    public void Move(float _speed)
    {
        //sub movement        
        if (_inputs == Vector2.zero)
        {
            if (rb.velocity != Vector3.zero) //slow down sub by -velocity
                rb.AddForce(-rb.velocity * _speed);
        }
        else //move Sub along z and x axis
        {
            rb.AddForce((transform.forward * _inputs.y + transform.right * _inputs.x).normalized * _speed);
            if (rb.velocity.magnitude > maxVelocity)
                rb.velocity = rb.velocity.normalized * maxVelocity;
        }

        if (mouseWheelInput != 0f)
        {
            rb.AddForce(new Vector3(0f, mouseWheelInput, 0f) * ballast);
            if (rb.velocity.magnitude > maxVelocity)
                rb.velocity = rb.velocity.normalized * maxVelocity;
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
            _spotlight.transform.rotation = RotationClamp(_spotlight.transform.rotation);
        }
    }

    Quaternion RotationClamp(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;
        q.z = 0.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
        angleX = Mathf.Clamp(angleX, -45, 45);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    public void SubRotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
                             Quaternion.Euler(0.0f, _lookStorage.x, 0f),
                             _turnSpeed * Time.fixedDeltaTime);
    }

    #region Grabber

    public void TryGrab()
    {
        //see if grab object
        _defaultIk = _defaulIKRestPos.transform;

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("GState"))
        {
            if (grabbedObject == null)
            {
                grabbedObject = GrabObject();
            }
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
                currentState = state.MOVEGRAB;
            }
            else
            {
                //reset Ik target
                _ik.solver.target = _defaultIk;
                _defaultIk = _defaulIKRestPos.transform;
            }
        }
    }
    GameObject GrabObject()
    {
        closest = null;
        Collider[] hitColliders = Physics.OverlapSphere(_defaulIKRestPos.transform.position, 2f, grabLayer);
        foreach (var hitCollider in hitColliders)
        {
            // hitCollider.gameObject.GetComponent<MeshRenderer>().material = _edgeGlow;
            if (Vector3.Distance(_defaulIKRestPos.transform.position, hitCollider.transform.position) < 2)
            {
                Vector3 dist = hitCollider.transform.position - _defaulIKRestPos.transform.position;
                float currentDistance = dist.sqrMagnitude;
                if (currentDistance < _distanceFromPickup)
                {
                    closest = hitCollider.gameObject;
                    _distanceFromPickup = currentDistance;
                }
                return closest;
            }
            else
                return closest;
        }
        return closest;
    }

    public void ReleaseGrab()
    {
        //turn on gravity from rigidbody?
        grabbedObject.transform.parent = null;
        grabbedObject.GetComponent<Rigidbody>().useGravity = true;
        grabbedObject = null;
        Cursor.lockState = CursorLockMode.Locked;
        //reset Ik target
        _ik.solver.target = _defaultIk;
        _defaultIk = _defaulIKRestPos.transform;
        currentState = state.TRYGRAB;
    }
    public void MoveGrab()
    {
        if (_lookCoOrds != Vector2.zero)
        {
            grabObjDistance += ((Vector3.right * _lookCoOrds.x) + (Vector3.up * _lookCoOrds.y)) * grabberSpeedH * Time.fixedDeltaTime;
        }
    }

    public void UpdateGrab()
    {
        grabbedObject.transform.localPosition = grabObjDistance;
        grabbedObject.transform.localRotation = Quaternion.identity;
    }
    #endregion
}


