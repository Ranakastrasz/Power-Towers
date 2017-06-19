using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerManager : MonoBehaviour {
    
    public Runner_Spawner Spawner;
    
    public GameObject RunnerType;


    private int _round;
    private int _toSpawn;

    //private int _minWave = 0;
    //private int _maxWave = 0;
    private Queue<int> spawnQueue = new Queue<int>();
    private bool spawning = false;

    private void QueueRunner(int iRound)
    {
        spawnQueue.Enqueue(iRound);
        if (!spawning)
        {
            SpawnRunner();
        }
    }

    private void QueueRunnerLoop()
    {
        QueueRunner(_round);
        _toSpawn--;
        if (_toSpawn <= 0)
        {
            CancelInvoke("QueueRunnerLoop");
        }
    }

    private void SpawnRunner()
    {
        int round = spawnQueue.Dequeue();
        int hitpoints = RunnerData.getRoundRunnerHealth(round, RunnerData.DIFFICULTY.NOOB);
        int bounty = RunnerData.getRoundRunnerBounty(round) * 2; // temp, till I get end-of-round bounty working again.

        /*GameObject runner = */Spawner.SpawnRunner(hitpoints, bounty, round);

        if (spawnQueue.Count > 0)
        {
            spawning = true;
            Invoke("SpawnRunner", Constants.MIN_SPAWN_PERIOD);
        }
        else
        {
            spawning = false;
        }

        //Player.Active.AddGold (RunnerData.getRoundFinishBounty(round));
        //Invoke("NextWave", Constants.ROUND_WAIT_TIME);
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

        InvokeRepeating("QueueRunnerLoop", 0f ,Constants.SPAWN_PERIOD);
        Invoke("NextWave", Constants.ROUND_WAIT_TIME + Constants.SPAWN_PERIOD * 10);
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
