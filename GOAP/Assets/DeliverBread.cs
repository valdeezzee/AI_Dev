﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverBread : GoapAction
{
    bool completed = false;
    float startTime = 0;
    public float workDuration = 2;

    public DeliverBread()
    {
        addPrecondition("hasDelivery", true);
        addEffect("doJob", true);
        name = "DeliverBread";
    }



    public override void reset()
    {
        completed = false;
        startTime = 0;
    }

    public override bool isDone()
    {
        return completed;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        return true;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            Debug.Log("Starting: " + name);
            startTime = Time.time;
        }

        if (Time.time - startTime > workDuration)
        {
            Debug.Log("Finishsed: " + name);
            this.GetComponent<Inventory>().breadLevel -= 5;
            completed = true;
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
