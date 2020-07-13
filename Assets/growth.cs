using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class growth : MonoBehaviour
{
    [SerializeField] GameObject _plantPrefab;
    Rigidbody _rb;
    GameObject _plant;
    Quaternion _rot;
    Vector3 _spawnPoint;
    public bool _canGrow;
    float _scale;
    float _destructTimer = 10;
    public bool _isSeaweed;
    // Start is called before the first frame update
    void Start()
    {
        _scale = Random.Range(0.2f, 1.5f);
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _destructTimer -= 1 * Time.deltaTime;
        if (_destructTimer <= 0) Destroy(this.gameObject);
        if (_canGrow)
        {
            _plant = (GameObject)Instantiate(_plantPrefab, _spawnPoint, _rot);
            _plant.transform.parent = this.transform.parent;
            _plant.transform.localScale = new Vector3(_scale, _scale, _scale);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
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
                _rot = Quaternion.FromToRotation(Vector3.up,Vector3.zero);
            _canGrow = true;
        }
    }
}
