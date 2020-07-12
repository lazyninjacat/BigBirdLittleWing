using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeding : MonoBehaviour
{

    public enum PlantType { Algae_Red, Anemone, SeaWeed }
    public PlantType _plantType;
    public Vector3 seedArea = new Vector3(2, 1, 2);
    static int numberAlgaeSeeds = 5000;
    public static int numbeSeaWeedSeeds = 500;
    public static int numberAnemoneSeeds = 10;

    [SerializeField] GameObject seedPrefab;
    GameObject[] AlgaeSeeds = new GameObject[numberAlgaeSeeds];
    GameObject[] AnemoneSeeds = new GameObject[numberAnemoneSeeds];
    GameObject[] SeaWeedSeeds = new GameObject[numbeSeaWeedSeeds];


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
                for (int i = 0; i < numberAlgaeSeeds; i++)
                {
                    Vector3 pos = new Vector3(Random.Range(transform.position.x - seedArea.x, transform.position.x + seedArea.x),
                                             Random.Range(transform.position.y - seedArea.y, transform.position.y - seedArea.y),
                                             Random.Range(transform.position.z - seedArea.z, transform.position.z + seedArea.z));

                    AlgaeSeeds[i] = (GameObject)Instantiate(seedPrefab, pos, Quaternion.identity);
                }
                break;
            case PlantType.Anemone:
                for (int i = 0; i < numberAnemoneSeeds; i++)
                {
                    Vector3 pos = new Vector3(Random.Range(transform.position.x - seedArea.x, transform.position.x + seedArea.x),
                                             Random.Range(transform.position.y - seedArea.y, transform.position.y - seedArea.y),
                                             Random.Range(transform.position.z - seedArea.z, transform.position.z + seedArea.z));

                    AnemoneSeeds[i] = (GameObject)Instantiate(seedPrefab, pos, Quaternion.identity);
                }
                break;
            case PlantType.SeaWeed:
                for (int i = 0; i < numbeSeaWeedSeeds; i++)
                {
                    Vector3 pos = new Vector3(Random.Range(transform.position.x - seedArea.x, transform.position.x + seedArea.x),
                                             Random.Range(transform.position.y - seedArea.y, transform.position.y - seedArea.y),
                                             Random.Range(transform.position.z - seedArea.z, transform.position.z + seedArea.z));

                    SeaWeedSeeds[i] = (GameObject)Instantiate(seedPrefab, pos, Quaternion.identity);
                }
                break;
            default:
                break;
        }
     
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    ///Rules
    ///if too close destroy one of the seedlings
    ///
}
