using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//generic camera movement script like what UNREAL has in its editor
public class DebugCamera : MonoBehaviour
{
    private Camera debugCamera;
    public bool isActive = false;

    public KeyCode ToggleControl = KeyCode.Backspace;
    public KeyCode forward = KeyCode.W;
    public KeyCode backward = KeyCode.S;
    public KeyCode right = KeyCode.D;
    public KeyCode left = KeyCode.A;
    public KeyCode up = KeyCode.Space;
    public KeyCode down = KeyCode.LeftShift;
    public KeyCode focus = KeyCode.F;

    public float speedVertical = 1f;
    public float speedHorizontal = 1f;
    public float speedUpDown = 1f;

    public float speedRotationX = 30f;
    public float speedRotationY = 30f;

    public float scrollWheelSpeed = 30f;
    [Tooltip("Don't set to main camera display View")]
    public int displayView = 1;
    private int mainDV;


    public bool didRay = false;
    public Vector3 rayTarget = Vector3.zero;

    private void Start()
    {
        //get camera component
        debugCamera = GetComponent<Camera>();
        //get main camera display
        mainDV = Camera.main.targetDisplay;

        //set default state
        if (isActive)
        {
            SetCameraOn();
        }
    }

    private void SetCameraOn()
    {
        debugCamera.targetDisplay = mainDV;
        Camera.main.targetDisplay = displayView;
        transform.position = Camera.main.transform.position;
        transform.rotation = Camera.main.transform.rotation;        
    }

    private void SetCameraOff()
    {
        //swap display views back
        debugCamera.targetDisplay = displayView;
        Camera.main.targetDisplay = mainDV;
        //debugCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        //toggle camera
        if(Input.GetKeyDown(ToggleControl))
        {
            isActive = !isActive;
            if (isActive)
            {
                SetCameraOn();
            }
            else
            {
                SetCameraOff();
            }
        }

        else if(isActive)
        {
            //Vector3 mousePosition = Input.mousePosition;
            //Debug.Log("Mouse Position: " + mousePosition);

            //focus rotation
            if(Input.GetMouseButton(0) && Input.GetKey(focus))
            {
                if(!didRay)
                {
                    didRay = true;
                    //only do this once...
                    RaycastHit hit;
                    //shoot a raycast into scene
                    if (Physics.Raycast(transform.position, transform.forward, out hit, 25f))
                    {
                        //whatever it hits becomes the focal point
                        rayTarget = hit.point;
                    }
                }

                if (rayTarget != Vector3.zero)
                {
                    //rotate around that object keeping view centered on that object
                }
            }
            //reset ray
            if(Input.GetKeyUp(KeyCode.F))
            {
                didRay = false;
            }
            //move in X/Z directions (world aligned) and rotate in the Y direction
            if(Input.GetMouseButton(0))
            {
                if(Input.GetAxis("Mouse X") != 0f)
                {
                    float y = Input.GetAxis("Mouse X") * speedRotationX * Time.deltaTime;
                    Vector3 t = new Vector3(0f, y, 0f);
                    transform.eulerAngles += t;
                }
                else
                {
                    Quaternion q = new Quaternion(0f, transform.rotation.y, 0f, 1f);
                    float dir = (Input.mousePosition.y - (Screen.height / 2f)) / (Screen.height / 2f);
                    transform.position += (dir * speedVertical * Time.deltaTime) * (q * Vector3.forward);
                }
            }
            else if(Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                Quaternion q = new Quaternion(0f, transform.rotation.y, 0f, 1f);
                transform.position += (Input.GetAxis("Mouse ScrollWheel") * scrollWheelSpeed * speedVertical * Time.deltaTime) * (q * Vector3.forward);
            }
            //x and y rotation
            else if(Input.GetMouseButton(1))
            {
                float x = Input.GetAxis("Mouse Y") * speedRotationY * Time.deltaTime;
                float y = Input.GetAxis("Mouse X") * speedRotationX * Time.deltaTime;
                Vector3 t = new Vector3(-x, y, 0f);
                transform.eulerAngles += t;
            }
            //mouse wheel press
            else if(Input.GetMouseButton(2))
            {
                Vector3 h = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y") * speedUpDown * Time.deltaTime, 0f);
                transform.position += h;
            }

            //keypadMovement
            if (Input.GetKey(forward))
            {
                transform.position += transform.forward * speedVertical * Time.deltaTime;
            }
            else if (Input.GetKey(backward))
            {
                transform.position -= transform.forward * speedVertical * Time.deltaTime;
            }
            if (Input.GetKey(right))
            {
                transform.position += transform.right * speedHorizontal * Time.deltaTime;
            }
            else if (Input.GetKey(left))
            {
                transform.position -= transform.right * speedHorizontal * Time.deltaTime;
            }
        }
    }

    

    //rotate according to mouse
    private void Rotation()
    {
        float x = Input.GetAxis("Mouse X") * speedRotationX * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * speedRotationY * Time.deltaTime;

        Quaternion q = new Quaternion(-y, x, 0f, 1f);

        transform.rotation *=  q;
        transform.rotation = new Quaternion(transform.rotation.x,
                                            transform.rotation.y,
                                            Mathf.Lerp(transform.rotation.z, 0f, 0.1f), 
                                            transform.rotation.w);

        //transform.eulerAngles = new Vector3(transform.eulerAngles.x,
        //                                    transform.eulerAngles.y,
        //                                    Mathf.Lerp(transform.eulerAngles.z, 0f, 0.1f));

        //Vector3 temp = new Vector3(x, y, 0f) + transform.rotation.eulerAngles;

        ////add 360 to x value
        //float xRot = temp.x;
        ////Debug.Log("X: " + x);

        //if (xRot > 90 && xRot < 180)
        //{
        //    xRot = 90;
        //}
        //else if (xRot < 360 + 0 && xRot > 180)
        //{
        //    xRot = 360 + 0;
        //}

        //transform.rotation = Quaternion.Euler(temp);
    }


    public void Movement()
    {
        Vector3 movement = Vector3.zero;

        if(Input.GetKey(forward))
        {
            movement += transform.forward * speedVertical * Time.deltaTime;
        }
        else if(Input.GetKey(backward))
        {
            movement -= transform.forward * speedVertical * Time.deltaTime;
        }
        if(Input.GetKey(right))
        {
            movement += transform.right * speedHorizontal * Time.deltaTime;
        }
        else if(Input.GetKey(left))
        {
            movement -= transform.right * speedHorizontal * Time.deltaTime;
        }
        if(Input.GetKey(up))
        {
            movement += transform.up * speedUpDown * Time.deltaTime;
        }
        else if(Input.GetKey(down))
        {
            movement -= transform.up * speedUpDown * Time.deltaTime;
        }

        transform.position += movement;
    }
}
