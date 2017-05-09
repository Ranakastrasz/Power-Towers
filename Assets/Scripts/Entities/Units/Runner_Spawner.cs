using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner_Spawner : Unit {
    // Spawner creates Runners on demand, and issues orders to them.
    // Also sets their stats.


    public GameObject RunnerGoal;


    // Also include a prototype later for when special Runners show up.
    public void SpawnRunner(int iHitpoints, int iBounty, int iRound)
    {
        EntityManager.CreateRunner(transform.position, iHitpoints, iBounty, RunnerGoal.GetComponent<Transform>(), iRound);

    }

    // Use this for initialization

    public new void Start ()
    {
        base.Start();
        //InvokeRepeating("SpawnRunner", 10.0f, 3.0f);
        
    }

    // Update is called once per frame
    public new void Update ()
    {
        base.Update();
    }
}
