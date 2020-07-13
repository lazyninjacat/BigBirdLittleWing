using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public GlobalFlock myManager;
    public float speedMult = 1;
    public float speed = 0.001f;
    float rotationSpeed = 4.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neighbourDistance = 2.5f;
    
    public bool turning = false;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.7f, 2);
    }

    // Update is called once per frame
    void Update()
    {
        Bounds b = new Bounds(myManager.transform.position, myManager.swimLimits * 2);  
        if (!b.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }
        if(turning)
        {
            Vector3 direction = myManager.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    rotationSpeed * Time.deltaTime);
            speed = Random.Range(0.7f,2)*speedMult;
        }
        else
        {
            if (Random.Range(0,5) < 1)
                ApplyRules();
        }
        

        transform.Translate(0, 0, Time.deltaTime * speed * speedMult);
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.allFish; 

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = myManager.goalPos;

        float dist;
        float pdist;
        int groupSize = 0;
        foreach (GameObject go in gos)
        {
            switch (myManager._gameType)
            {
                case GlobalFlock.GameType.Menu:
                    if (go != this.gameObject)
                    {
                        dist = Vector3.Distance(go.transform.position, this.transform.position);
                        if (dist <= neighbourDistance)
                        {
                            vcentre += go.transform.position;
                            groupSize++;

                            if (dist < 2.0f)
                            {
                                vavoid = vavoid + (this.transform.position - go.transform.position);
                            }
                            Flock anotherFlock = go.GetComponent<Flock>();
                            gSpeed = gSpeed + anotherFlock.speed;
                        }
                    }
                    break;
                case GlobalFlock.GameType.Game:
                    if (go != this.gameObject)
                    {
                        dist = Vector3.Distance(go.transform.position, this.transform.position);
                        pdist = Vector3.Distance(this.transform.position, myManager._playerPos.position);
                        if (dist <= neighbourDistance)
                        {
                            vcentre += go.transform.position;
                            groupSize++;

                            if (dist < 2.0f)
                            {
                                vavoid = vavoid + (this.transform.position - go.transform.position);
                            }
                            Flock anotherFlock = go.GetComponent<Flock>();
                            gSpeed = gSpeed + anotherFlock.speed;
                        }
                        if (pdist <= neighbourDistance)
                        {
                            vcentre += myManager._playerPos.position + go.transform.position;
                            groupSize++;

                            if (pdist < 2.0f)
                            {
                                vavoid = vavoid + (this.transform.position - myManager._playerPos.position);
                            }
                            Flock anotherFlock = go.GetComponent<Flock>();
                            gSpeed = gSpeed + anotherFlock.speed + myManager._playerPos.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
                        }
                    }
                    break;
                default:
                    break;
            }
           
        }

        if(groupSize > 0)
        {
            vcentre = vcentre / groupSize + (goalPos - this.transform.position);
            speed = gSpeed / groupSize * speedMult;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                       Quaternion.LookRotation(direction),
                                                       rotationSpeed * Time.deltaTime);
        }
  
    }
}
