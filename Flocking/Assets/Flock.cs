using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{

    public FlockManager myManager;
    float speed;
    bool turning = false;
    Animator anim;
    // Use this for initialization
    void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);

        anim = this.GetComponent<Animator>();
        anim.SetFloat("swimOffset", Random.Range(0.0f, 1.0f));
        anim.SetFloat("swimSpeed", speed);
    }

    // Update is called once per frame
    void Update()
    {
        // determine the bounding box of the manager cube
        Bounds b = new Bounds(myManager.transform.position, myManager.swimLimits * 2);

        // if fish is outside the bounds of the cube of about to hit something 
        // then start turning around
        RaycastHit hit = new RaycastHit();
        Vector3 direction = Vector3.zero;


        if (!b.Contains(transform.position)) // if fish is out of bounds
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        else if(Physics.Raycast(transform.position, this.transform.forward * 50, out hit)) // if fish hits something
        {
            turning = true;

            // forward vector would be the incoming vector
            // hit give alot of information. it gives of normal and other useful information
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
            //Debug.DrawRay(this.transform.position, this.transform.forward * 50, Color.red);
        }
        else
        {
            turning = false;
        }

        // calculate new direction to turn towards
        if(turning)
        {
            // turn towards center of manager cube
            
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);

            if (Random.Range(0, 100) < 20)
                ApplyRules();

            anim.SetFloat("swimSpeed", speed);
        }

        
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.allFish;

        Vector3 vcentre = Vector3.zero; // average center of group
        Vector3 vavoid = Vector3.zero; // average avoidance vector
        float gSpeed = 0.01f; // this will hold the average speed
        float nDistance; // check to see another fish will be considered as part of your group
        int groupSize = 0;


        foreach(GameObject go in gos)
        {
            if(go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if(nDistance <= myManager.neighbourDistance)
                {
                    vcentre += go.transform.position; // add each neighbor fish vector
                    groupSize++;

                    if(nDistance < 1.0f) // How close we are allowd to be to another fish before we should avoid it
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed; // get the average speed
                }
            }
        }

        if(groupSize > 0)
        {
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position; // calculates the direction the fish should be going
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                        Quaternion.LookRotation(direction),
                                                        myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
