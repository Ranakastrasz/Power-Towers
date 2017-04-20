using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creep_Spawner : Unit {
    // Spawner creates creeps on demand, and issues orders to them.
    // Also sets their stats.


    public GameObject CreepGoal;
    private double hitpoints = 25.0;
    private int creepCount = 0;

    private void spawnCreep()
    {
        creepCount++;
        GameObject obj = EntityManager.CreateCreep(transform.position, (int)hitpoints, ((creepCount-1)/20)+1, CreepGoal.GetComponent<Transform>());
        Creep creep = obj.GetComponent<Creep>();
        hitpoints = hitpoints * 1.05;

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
