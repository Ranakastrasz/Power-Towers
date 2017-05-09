using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {
    
    public Runner_Spawner spawner;
    
    public GameObject RunnerType;


    private int toSpawn;
    private int hitpoints;
    private int bounty;

    // Temp 
    private int round = 0;



    private void SpawnRunner()
    {
        spawner.SpawnRunner(hitpoints, bounty, round);
        toSpawn--;
        if (toSpawn > 0)
        {
            Invoke("SpawnRunner", Constants.SPAWN_PERIOD);
        }
        else
        {
			Player.Active.AddGold (RunnerData.getRoundFinishBounty(round));
            Invoke("NextWave", Constants.ROUND_WAIT_TIME);
        }
    }


    // Temp
    public void NextWave()
    {
        Init(RunnerType, spawner, round+1);
    }

    public void Init(GameObject iRunnerType, Runner_Spawner iSpawner, int iRound)
    {
        round = iRound;
        toSpawn = Constants.SPAWNS_PER_ROUND;
        hitpoints = RunnerData.getRoundRunnerHealth(round,RunnerData.DIFFICULTY.NOOB);
        bounty = RunnerData.getRoundRunnerBounty(round);

        Invoke("SpawnRunner", Constants.MIN_SPAWN_PERIOD);
    }


	void Start ()
	{
		
		Invoke("NextWave", Constants.ROUND_WAIT_TIME);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
