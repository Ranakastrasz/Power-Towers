using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{

    public const float SELL_PERCENTAGE = 1.0f; //remember to update tool tips if this is changed
    public const int STARTING_GOLD     = 25; //gold given to players at start of game
     
    public const int MAX_ROUND         = 30; //number of rounds.
     
    //=== Game ===
    public const float ROUND_WAIT_TIME         = 30.0f; //seconds between rounds
    public const float RUSHED_ROUND_WAIT_TIME  = 10.0f; //seconds between rounds in rushed mode
    public const float SELECTION_TIME          = 150.0f; //seconds
    public const int SPAWNS_PER_ROUND          = 10;
    public const float SPAWN_PERIOD            = 4.5f; //time between normal spawns
    public const float MIN_SPAWN_PERIOD        = 0.5f; //minimum time between spawns (for war/race)
    public const int MAX_ROUNDS                = 500;
	//=== Ability Behaviour ===
	public const int MAX_TOWER_TRANSFERS       = 8;


}
