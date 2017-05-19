using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {
    
    public Runner_Spawner Spawner;
    
    public GameObject RunnerType;


    private int _toSpawn;
    private int _hitpoints;
    private int _bounty;

    // Temp 
    private int _round = 0;



    private void SpawnRunner()
    {
        Spawner.SpawnRunner(_hitpoints, _bounty, _round);
        _toSpawn--;
        if (_toSpawn > 0)
        {
            Invoke("SpawnRunner", Constants.SPAWN_PERIOD);
        }
        else
        {
			Player.Active.AddGold (RunnerData.getRoundFinishBounty(_round));
            Invoke("NextWave", Constants.ROUND_WAIT_TIME);
        }
    }


    // Temp
    public void NextWave()
    {
        Init(RunnerType, Spawner, _round+1);
    }

    public void Init(GameObject iRunnerType, Runner_Spawner iSpawner, int iRound)
    {
        _round = iRound;
        _toSpawn = Constants.SPAWNS_PER_ROUND;
        _hitpoints = RunnerData.getRoundRunnerHealth(_round,RunnerData.DIFFICULTY.NOOB);
        _bounty = RunnerData.getRoundRunnerBounty(_round);

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
