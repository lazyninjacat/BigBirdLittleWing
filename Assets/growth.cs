using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class growth : MonoBehaviour
{
    [SerializeField] GameObject[] _plantPrefab;
    Rigidbody _rb;
    GameObject _plant;
    Quaternion _rot;
    Vector3 _spawnPoint;
    public bool _canGrow;
    float _scale;
    Vector3 _cache;
    float _destructTimer = 5;
    public bool _isSeaweed;
    public bool _isColor;
    // Start is called before the first frame update
    void Start()
    {
        _scale = Random.Range(0.2f, 1.5f);
        _rb = GetComponent<Rigidbody>();
        _cache = new Vector3(_scale, _scale, _scale);
    }

    private void OnEnable()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        _destructTimer -= 1 * Time.deltaTime;
        if (_destructTimer <= 0) Destroy(this.gameObject);
        if (_canGrow)
        {
            if (_isColor)
            {
                _plant = (GameObject)Instantiate(_plantPrefab[Random.Range(0,_plantPrefab.Length)], _spawnPoint, _rot);
                _plant.transform.parent = this.transform.parent;
                _plant.transform.localScale = _cache;
                Destroy(this.gameObject);
            }
            else
            {
                _plant = (GameObject)Instantiate(_plantPrefab[0], _spawnPoint, _rot);
                _plant.transform.parent = this.transform.parent;
                _plant.transform.localScale = _cache;
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float dist = Vector3.Distance(transform.position, collision.transform.position);
        float rad = GetComponent<SphereCollider>().radius;
        if (collision.collider.gameObject.layer == 12)
        {
            if (dist < rad * 2)
            {
                _cache = collision.transform.localScale / Random.Range(1.5f, 2);
            }
            else if (dist < rad)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _cache = collision.transform.localScale / Random.Range(1.3f, 1.6f);
            }
        }
        else
        {
            ContactPoint spawn = collision.contacts[0];
            if (!_canGrow)
            {
                _rb.isKinematic = true;
                _spawnPoint = spawn.point;
                _spawnPoint.y = _spawnPoint.y + 0.1f;
                if (!_isSeaweed)
                {
                    _rot = Quaternion.FromToRotation(Vector3.up, spawn.normal);
                }
                else
                {
                    _rot = Quaternion.FromToRotation(Vector3.up, Vector3.zero);
                }
                _canGrow = true;
            }
        }
    }
}
