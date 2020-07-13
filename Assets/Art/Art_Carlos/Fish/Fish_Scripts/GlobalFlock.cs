using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    public GlobalFlock myFlock;
    public GameObject fishprefab;
    static int numFish = 8;
    public  GameObject[] allFish = new GameObject[numFish];
    public Vector3 goalPos;
    public Vector3 swimLimits = new Vector3(5, 5, 5);
    PlayerManager _player;
    public Transform _playerPos;

    public enum GameType { Menu, Game};
    public GameType _gameType = GameType.Game;
    public void FishSpeed(float speedMult)
    {
       // Debug.Log(speedMult);
        for (int i = 0; i < numFish; i++)
        {
            allFish[i].GetComponent<Flock>().speedMult = speedMult;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(swimLimits.x * 2, swimLimits.y * 2, swimLimits.z * 2));
        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawSphere(goalPos, 0.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        myFlock = this;
        goalPos = this.transform.position;
        _player = FindObjectOfType<PlayerManager>();
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = new Vector3(Random.Range(transform.position.x - swimLimits.x,transform.position.x - swimLimits.x),
                                     Random.Range(transform.position.y - swimLimits.y, transform.position.y - swimLimits.y),
                                     Random.Range(transform.position.z - swimLimits.z, transform.position.z + swimLimits.z));

            allFish[i] = (GameObject)Instantiate(fishprefab, pos, Quaternion.identity);
            allFish[i].transform.parent = this.transform.parent.transform.parent;
            allFish[i].GetComponent<Flock>().myManager = this;
        }


    }

    // Update is called once per frame
    void Update()
    {
        switch (_gameType)
        {
            case GameType.Menu:
                if (Random.Range(0, 100000) < 5)
                {
                    goalPos = new Vector3(Random.Range(transform.position.x - swimLimits.x, transform.position.x + swimLimits.x),
                                          Random.Range(transform.position.y - swimLimits.y, transform.position.y + swimLimits.y),
                                          Random.Range(transform.position.z - swimLimits.z, transform.position.z + swimLimits.z));
                }
                break;
            case GameType.Game:
                _playerPos = _player.currentSub.transform;
                if (Random.Range(0, 100000) < 5)
                {
                    goalPos = new Vector3(Random.Range(transform.position.x - swimLimits.x, transform.position.x + swimLimits.x),
                                          Random.Range(transform.position.y - swimLimits.y, transform.position.y + swimLimits.y),
                                          Random.Range(transform.position.z - swimLimits.z, transform.position.z + swimLimits.z));
                }
                break;
            default:
                break;
        }
     
    }
}
