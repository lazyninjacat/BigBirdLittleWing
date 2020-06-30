//Written by: Kyle J. Ennis
//last edited 14/10/19
//last edited 14/11/19

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Camera/Player Camera", 0)]
[RequireComponent(typeof(Camera))]

public class PlayerCamera : MonoBehaviour
{

    #region Player
    [SerializeField] GameObject Player;
    Rigidbody p_RB;
    #endregion
    #region UI and Player Choices
    [Header("Turn on/off the mouse cursor")]
    public bool lockCursor = false;
    [Header("Invert Camera Y")]
    public bool invY;
    [Header("Camera Sensitivity and rotation smoothing")]
    [Tooltip("Adjusts camera movement sensitivity.")]
    [Range(1, 10)]
    public float sensitivity = 2.5f;
    [Tooltip("Adjusts how quickly the camera moves acording to user input.")]
    [Range(0, 1)]
    public float rotationsmoothTime = 0.202f;
    #endregion

    #region Camera Distance and Orbit Management
    float YAxis;
    [SerializeField] private float zoomTarget;
    [SerializeField] private float zoomLerpTime;
    float camDistMin = 10;
    [SerializeField] Vector2 pitchMinMax;
    Vector3 currentRotation;
    Vector3 smoothingVelocity;
    float yaw;
    float pitch;
    #endregion

    #region CameraRayCasts
    Camera main;
    GameObject aimer;
    public LayerMask IgnoreMask;
    RaycastHit hit;
    [SerializeField] float cameraOffsetX;
    [SerializeField] float cameraOffsetY;
    [SerializeField] float cameraOffsetZ;
    float camDist;

    #endregion

    enum CameraType { Orbit }
   


    private void Awake()
    {
        //if (!pl) pl = FindObjectOfType<P_Movement>();
        if (!aimer) aimer = GameObject.FindGameObjectWithTag("CameraAimer");
        if (!main) main = Camera.main;
        if (!p_RB) p_RB = Player.GetComponent<Rigidbody>();
    }
    void Start()
    {
       
        currentRotation = Vector3.SmoothDamp(currentRotation, Player.transform.eulerAngles, ref smoothingVelocity, rotationsmoothTime);
        if (lockCursor) Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
      
        camDist = UpdateZoom();
    }
    private void LateUpdate()
    {
        CamMovement3D();
    }

    //Basic Camera Movements, Orbit around player and joystick control, as well as player offset
    void CamMovement3D()
    {
        YAxis = Input.GetAxis("Mouse Y") * sensitivity;
        pitch = (invY) ? pitch += YAxis : pitch -= YAxis;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref smoothingVelocity, rotationsmoothTime);
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        transform.eulerAngles = currentRotation;
        transform.position = Player.transform.position - (transform.forward - (transform.right * cameraOffsetX) + (transform.up * cameraOffsetY)) * camDist;
    }
    #region CameraUtilities

    private float UpdateZoom()
    {
        zoomTarget = Player.transform.localScale.y * 8;
        if (main.orthographicSize != zoomTarget)
        {
            float target = Mathf.Lerp(main.orthographicSize, zoomTarget, zoomLerpTime * Time.deltaTime);
            main.orthographicSize = Mathf.Clamp(target, camDistMin, Mathf.Infinity);
        }
        return main.orthographicSize;
    }

    //Checks cameras distance from the player and adjusts accordingly
    float CameraRaycast(float x)
    {
        if (Physics.Raycast(aimer.transform.position, main.transform.position - aimer.transform.position, out hit, x, IgnoreMask))
            return hit.distance - 0.2f;
        else
            return x;
    }

    //needs to be tied to menu system for adjustment by player
    public void switchInverseY()
    {
        invY = !invY;
    }

    #endregion
}

