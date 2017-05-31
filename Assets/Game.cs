using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	// Current Difficulty
	// Current Gamestate
	// RoundCount
	// LiveBase
	// RushMode
	// TechMode

	public enum DIFFICULTY
	{
		NOOB,
		ROOKIE,
		HOTSHOT,
		VETERAN,
		ELITE,
		PSYCHO
	}
	public enum GAME_STATE
	{
		MENU, // At Main Menu.
		RUNNING, // In Game
		DONE // Player is dead.
	}

	public DIFFICULTY difficulty { protected set; get; }
	public GAME_STATE state      { protected set; get; }

	public int roundCount { protected set; get; }
	public int liveBase   { protected set; get; }
	public bool rushMode  { protected set; get; }
	public bool techMode  { protected set; get; }

	// Use this for initialization
	void Start () {
		difficulty = DIFFICULTY.NOOB;
		state = GAME_STATE.MENU;
		roundCount = 15;
		liveBase = 25;
		rushMode = false;
		techMode = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
