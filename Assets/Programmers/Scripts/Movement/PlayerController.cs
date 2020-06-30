using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PlayerController : MonoBehaviour
{
    [SerializeField] public Transform body;
    [SerializeField] public Transform cameraPosition;
    public Rigidbody rb;
    public Animator anim;
    
    protected Vector3 _objPos;
    [SerializeField] protected Vector2 _inputs;    

    public float _lookSensitivity = 2f;
    public float speed = 0.5f;
    public float maxVelocity = 2f;
    public Vector2 _lookCoOrds;
    public Vector2 _lookStorage;
    public float _turnSpeed = 2;

    //basic run function
    public abstract void RunUpdate();
    public abstract void RunFixedUpdate();
    public abstract void RunLateUpdate();
    public abstract void Walking(float speed, GameObject obj);

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
}
