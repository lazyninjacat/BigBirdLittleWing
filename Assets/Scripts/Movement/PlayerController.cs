using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    
    protected Vector3 _objPos;
    [SerializeField] protected Vector2 _inputs;    

    public float _lookSensitivity = 2f;
    public float speed = 0.5f;
    public float maxVelocity = 2f;
    public Vector2 _lookCoOrds;
    public Vector2 _lookStorage;
    public float _turnSpeed = 4;
    public bool _inv;

    //basic run function
    public abstract void RunUpdate();
    public abstract void RunFixedUpdate();
    public abstract void RunLateUpdate();

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

}
