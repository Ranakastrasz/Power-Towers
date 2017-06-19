using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner_Spawner : MonoBehaviour {
    // Spawner creates Runners on demand, and issues orders to them.
    // Also sets their stats.


    public GameObject _runnerGoal;


    // Also include a prototype later for when special Runners show up.
    public GameObject SpawnRunner(int iHitpoints, int iBounty, int iRound)
    {
        return EntityManager.CreateRunner(transform.position, iHitpoints, iBounty, _runnerGoal.GetComponent<Transform>(), iRound);

    }

    // Use this for initialization
    
}
