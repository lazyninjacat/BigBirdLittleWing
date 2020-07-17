using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    //[Range(0, 100)]
    //public float _radius = 20;

    //[Range(0, 20)]
    //public float _angle = 20;

    //[Range(0,60)]
    //public int _segments = 50;
    //public float[] positions;
    //LineRenderer line;
    //public LayerMask _radarPing;
    public Camera camera;
    //void Start()
    //{
    //    //line = gameObject.GetComponent<LineRenderer>();

    //    //line.positionCount =_segments + 1;
    //    //line.useWorldSpace = false;
  
    //}

    public Rect rect;
    public Material mat;
    Texture2D texture;
    void Start()
    {
        camera.pixelRect = rect;
        texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);
        mat.mainTexture = texture;
    }

    void OnPostRender()
    {
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();
    }
    private void Update()
    {

        camera.pixelRect = rect;
        texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);
        mat.mainTexture = texture;

        //Collider[] hitColliders = Physics.OverlapSphere(transform.position, 100f, _radarPing);
        //for (int i = 0; i < hitColliders.Length; i++)
        //{
        //    float x = Vector3.Distance(transform.position, hitColliders[i].transform.position);
        //    positions[i] = x;
        //}

        //    _angle = 20.0f;
        //CreatePoints();
    }
    //void CreatePoints()
    //{
    //    float x;
    //    float y;

    

    //    for (int i = 0; i < (_segments + 1); i++)
    //    {
    //        x = Mathf.Sin(Mathf.Deg2Rad * _angle) * _radius;
    //        y = Mathf.Cos(Mathf.Deg2Rad * _angle) * _radius;

    //        line.SetPosition(i, new Vector3(x, y, 0));

    //        _angle += (360f / _segments);
    //    }
    //}

}
