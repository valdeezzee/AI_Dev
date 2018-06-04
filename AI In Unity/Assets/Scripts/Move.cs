using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public Vector3 goal = new Vector3(5, 0, 4);
    public float speed = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    // LateUpdate is good for physics and movement
    // runs after all the physics have been calculated in the scene
	void LateUpdate ()
    {
        this.transform.Translate(goal.normalized * speed * Time.deltaTime); //Time.delatTime smooths out and allows for the difference in the update running
	}
}
