using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Docking : MonoBehaviour
{
    [SerializeField] Transform Dock;
    private Vector3 DockPosition;


    void Update()
    {
        DockPosition = new Vector3(Dock.position.x, Dock.position.y, Dock.position.z);

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            Debug.Log("Docking at " + DockPosition);
            gameObject.transform.position = DockPosition;
        }
    }
}
