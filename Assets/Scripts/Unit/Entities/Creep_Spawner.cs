using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creep_Spawner : Unit {
    // Spawner creates creeps on demand, and issues orders to them.
    // Also sets their stats.


    public GameObject CreepGoal;
    public GameObject CreepPrefab;
    public GameObject CreepContainer;
    private double hitpoints = 5.0;

    private void spawnCreep()
    {

        GameObject myCreep = (GameObject)
            Instantiate(CreepPrefab, transform.position, Quaternion.identity);
        myCreep.GetComponent<Creep>().Init((int)hitpoints, 1, CreepGoal.GetComponent<Transform>());
        hitpoints = hitpoints * 1.05;
        myCreep.transform.parent = CreepContainer.transform;
    }

    // Use this for initialization

    public new void Start ()
    {
        base.Start();
        InvokeRepeating("spawnCreep", 3.0f, 3.0f);
        
    }

    // Update is called once per frame
    public new void Update ()
    {
        base.Update();
    }
}
