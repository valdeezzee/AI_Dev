using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow : MonoBehaviour {

    // Change circuit.Waypoints to waypoints to use own code
    //public GameObject[] waypoints;
    public UnityStandardAssets.Utility.WaypointCircuit circuit;
    int currentWp = 0;

    float speed = 3.0f;
    float accuracy = 0.2f;
    float rotSpeed = 0.6f;
	// Use this for initialization
	void Start () {
        //waypoints = GameObject.FindGameObjectsWithTag("waypoint");
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        if (circuit.Waypoints.Length == 0) return;

        Vector3 lookAtGoal = new Vector3(circuit.Waypoints[currentWp].transform.position.x,
                                        this.transform.position.y,
                                        circuit.Waypoints[currentWp].transform.position.z);
        Vector3 direction = lookAtGoal - this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                Quaternion.LookRotation(direction),
                                                Time.deltaTime * rotSpeed);// turns the tank towards the waypoint
        if(direction.magnitude < accuracy)
        {
            currentWp++;
            if(currentWp >= circuit.Waypoints.Length)
            {
                currentWp = 0;
            }
        }
        this.transform.Translate(0, 0, speed * Time.deltaTime);// pushing tank forward in the z direction
	}
}
