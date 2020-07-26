using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.PlayerLoop;

public class Seeding : MonoBehaviour
{

    public enum PlantType { Algae_Red, Anemone, SeaWeed, Fish, Coral, Rock }
    public PlantType _plantType;
    public Vector3 _Bounds = new Vector3(2, 1, 2);
    static int _maxObj = 1000;
    Camera _mainCam;
    [SerializeField] GameObject[] _spawnPrefab;
    GameObject[] _objIndex = new GameObject[_maxObj];
    public int _spawnCount;
    PlayerManager _player;
    RaycastHit hit;
    public LayerMask _layer = 1 << 14;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawCube(transform.position, new Vector3(_Bounds.x * 2, _Bounds.y * 2, _Bounds.z * 2)); 
    }

    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        for (int i = 0; i < _spawnCount; i++)
        {
            Quaternion _rot;
            Vector3 pos = new Vector3(Random.Range(transform.position.x - _Bounds.x, transform.position.x + _Bounds.x),
                                     Random.Range(transform.position.y - _Bounds.y, transform.position.y + _Bounds.y),
                                     Random.Range(transform.position.z - _Bounds.z, transform.position.z + _Bounds.z));

            switch (_plantType)
            {
                case PlantType.Algae_Red:
                    Physics.Raycast(pos, -Vector3.up, out hit, Mathf.Infinity, 1 << 14);
                    _objIndex[i] = (GameObject)Instantiate(_spawnPrefab[Random.Range(0, _spawnPrefab.Length)], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                    _objIndex[i].transform.parent = this.transform;               
                    _objIndex[i].layer = 12;
                    CheckNeighbours(12,i);
                    break;
                case PlantType.Anemone:
                    Physics.Raycast(pos, -Vector3.up, out hit, Mathf.Infinity, 1 << 14);
                    _objIndex[i] = (GameObject)Instantiate(_spawnPrefab[Random.Range(0, _spawnPrefab.Length)], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                    _objIndex[i].transform.parent = this.transform;
                    _objIndex[i].layer = 10;
                    CheckNeighbours(10,i);
                    break;
                case PlantType.SeaWeed:
                    Physics.Raycast(pos, -Vector3.up, out hit, Mathf.Infinity, 1 << 14);
                    _objIndex[i] = (GameObject)Instantiate(_spawnPrefab[Random.Range(0, _spawnPrefab.Length)], hit.point, Quaternion.FromToRotation(Vector3.up, Vector3.zero));
                    _objIndex[i].transform.parent = this.transform;
                    _objIndex[i].layer = 12;
                    CheckNeighbours(12,i);
                    break;
                case PlantType.Fish:
                    _objIndex[i] = (GameObject)Instantiate(_spawnPrefab[0], pos, Quaternion.identity);
                    _objIndex[i].transform.parent = this.transform;
                    _objIndex[i].layer = 11;
                    break;
                case PlantType.Coral:
                    Physics.Raycast(pos, -Vector3.up, out hit, Mathf.Infinity, 1 << 14);
                    _objIndex[i] = (GameObject)Instantiate(_spawnPrefab[Random.Range(0, _spawnPrefab.Length)], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                    _objIndex[i].transform.parent = this.transform;
                    _objIndex[i].layer = 12;
                    CheckNeighbours(12,i);
                    break;
                case PlantType.Rock:
                    Physics.Raycast(pos, -Vector3.up, out hit, Mathf.Infinity, 1 << 14);
                    _objIndex[i] = (GameObject)Instantiate(_spawnPrefab[Random.Range(0, _spawnPrefab.Length)], hit.point, Quaternion.FromToRotation(Vector3.up, Vector3.zero));
                    _objIndex[i].transform.parent = this.transform;
                    _objIndex[i].layer = 14;
                    break;
                default:
                    break;
            }
            _objIndex[i].transform.localScale = transform.localScale / Random.Range(0.2f, 2);
            _rot = _objIndex[i].transform.rotation;
            _rot.y = Random.Range(0, 360);
            transform.rotation = _rot;
          
        }
    }

    void CheckNeighbours(int x, int i)
    {
        Collider[] colliders = Physics.OverlapSphere(_objIndex[i].transform.position, 0.5f, 1 << x);
        foreach (var item in colliders)
        {
            if (Random.Range(0, 100) < 2)
            {
                item.gameObject.SetActive(false);
            }
            else
                item.transform.localScale = transform.localScale / Random.Range(1.3f, 2);
        }
    }
    private void Update()
    {
        switch (_plantType)
        {
            case PlantType.Algae_Red:
                break;
            case PlantType.Anemone:
                break;
            case PlantType.SeaWeed:
                break;
            case PlantType.Fish:
                if (Random.Range(0, 10000) < 50)
                {
                    for (int i = 0; i < _spawnCount; i++)
                    {
                        Vector3 pos = new Vector3(Random.Range(transform.position.x - _Bounds.x, transform.position.x + _Bounds.x),
                                                 Random.Range(transform.position.y - _Bounds.y, transform.position.y + _Bounds.y),
                                                 Random.Range(transform.position.z - _Bounds.z, transform.position.z + _Bounds.z));
                        _objIndex[i].transform.position = pos;
                    }
                }
                break;
            case PlantType.Coral:
                break;
            default:
                break;
        }

    }
}
