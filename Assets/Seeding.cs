using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Seeding : MonoBehaviour
{

    public enum PlantType { Algae_Red, Anemone, SeaWeed, Fish }
    public PlantType _plantType;
    public Vector3 seedArea = new Vector3(2, 1, 2);
    static int _maxSeeds = 1000;

    [SerializeField] GameObject seedPrefab;
    GameObject[] AlgaeSeeds = new GameObject[_maxSeeds];
    GameObject[] AnemoneSeeds = new GameObject[_maxSeeds];
    GameObject[] SeaWeedSeeds = new GameObject[_maxSeeds];
    GameObject[] FishEggs = new GameObject[_maxSeeds];
    public int _seedNumber;

    public float seedingDistance;
      private void OnDrawGizmosSelected()
    {       
        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawCube(transform.position, new Vector3(seedArea.x * 2, seedArea.y * 2, seedArea.z * 2));
    }
    // Start is called before the first frame update
    void Start()
    {
        switch (_plantType)
        {
            case PlantType.Algae_Red:
                for (int i = 0; i < _seedNumber; i++)
                {
                    Vector3 pos = new Vector3(Random.Range(transform.position.x - seedArea.x, transform.position.x + seedArea.x),
                                             Random.Range(transform.position.y - seedArea.y, transform.position.y + seedArea.y),
                                             Random.Range(transform.position.z - seedArea.z, transform.position.z + seedArea.z));

                    AlgaeSeeds[i] = (GameObject)Instantiate(seedPrefab, pos, Quaternion.identity);
                    AlgaeSeeds[i].transform.parent = this.transform;
                }
                break;
            case PlantType.Anemone:
                for (int i = 0; i < _seedNumber; i++)
                {
                    Vector3 pos = new Vector3(Random.Range(transform.position.x - seedArea.x, transform.position.x + seedArea.x),
                                             Random.Range(transform.position.y - seedArea.y, transform.position.y + seedArea.y),
                                             Random.Range(transform.position.z - seedArea.z, transform.position.z + seedArea.z));

                    AnemoneSeeds[i] = (GameObject)Instantiate(seedPrefab, pos, Quaternion.identity);
                    AnemoneSeeds[i].transform.parent = this.transform;

                }
                break;
            case PlantType.SeaWeed:
                for (int i = 0; i < _seedNumber; i++)
                {
                    Vector3 pos = new Vector3(Random.Range(transform.position.x - seedArea.x, transform.position.x + seedArea.x),
                                             Random.Range(transform.position.y - seedArea.y, transform.position.y + seedArea.y),
                                             Random.Range(transform.position.z - seedArea.z, transform.position.z + seedArea.z));

                    SeaWeedSeeds[i] = (GameObject)Instantiate(seedPrefab, pos, Quaternion.identity);
                    SeaWeedSeeds[i].GetComponent<growth>()._isSeaweed = true;
                    SeaWeedSeeds[i].transform.parent = this.transform;
                }
                break;
            case PlantType.Fish:
                for (int i = 0; i < _seedNumber; i++)
                {
                    Vector3 pos = new Vector3(Random.Range(transform.position.x - seedArea.x, transform.position.x + seedArea.x),
                                             Random.Range(transform.position.y - seedArea.y, transform.position.y + seedArea.y),
                                             Random.Range(transform.position.z - seedArea.z, transform.position.z + seedArea.z));

                    SeaWeedSeeds[i] = (GameObject)Instantiate(seedPrefab, pos, Quaternion.identity);
                    SeaWeedSeeds[i].transform.parent = this.transform;
                }
                break;
            default:
                break;
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
                    for (int i = 0; i < _seedNumber; i++)
                    {
                        Vector3 pos = new Vector3(Random.Range(transform.position.x - seedArea.x, transform.position.x + seedArea.x),
                                                 Random.Range(transform.position.y - seedArea.y, transform.position.y + seedArea.y),
                                                 Random.Range(transform.position.z - seedArea.z, transform.position.z + seedArea.z));

                        SeaWeedSeeds[i].transform.position = pos;
                    }
                }
                break;
            default:
                break;
        }
      
    }
    ///Rules
    ///if too close destroy one of the seedlings
    ///
}
