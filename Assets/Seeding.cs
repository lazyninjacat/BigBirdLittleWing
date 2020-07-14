using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.PlayerLoop;

public class Seeding : MonoBehaviour
{

    public enum PlantType { Algae_Red, Anemone, SeaWeed, Fish, Coral }
    public PlantType _plantType;
    public Vector3 _Bounds = new Vector3(2, 1, 2);
    static int _maxObj = 1000;

    [SerializeField] GameObject _spawnPrefab;
    GameObject[] _objIndex = new GameObject[_maxObj];
    public int _spawnCount;
      private void OnDrawGizmosSelected()
    {       
        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawCube(transform.position, new Vector3(_Bounds.x * 2, _Bounds.y * 2, _Bounds.z * 2));
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(transform.position.x - _Bounds.x, transform.position.x + _Bounds.x),
                                     Random.Range(transform.position.y - _Bounds.y, transform.position.y + _Bounds.y),
                                     Random.Range(transform.position.z - _Bounds.z, transform.position.z + _Bounds.z));

            _objIndex[i] = (GameObject)Instantiate(_spawnPrefab, pos, Quaternion.identity);
            _objIndex[i].transform.parent = this.transform;

            switch (_plantType)
            {
                case PlantType.Algae_Red:
                    _objIndex[i].GetComponent<growth>()._isColor = true;
                    _objIndex[i].layer = 12;
                    break;
                case PlantType.Anemone:
                    _objIndex[i].layer = 10;
                    break;
                case PlantType.SeaWeed:
                    _objIndex[i].layer = 12;
                    _objIndex[i].GetComponent<growth>()._isSeaweed = true;
                    break;
                case PlantType.Fish:
                    _objIndex[i].layer = 11;
                    break;
                case PlantType.Coral:
                    _objIndex[i].layer = 12;
                    break;
                default:
                    break;
            }
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
    ///Rules
    ///if too close destroy one of the seedlings
    ///
}
