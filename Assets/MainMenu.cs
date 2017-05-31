using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	/*
Difficulties - Creep HP base
-Noob
-Rookie
-Hotshot
-Veteran
-Psycho

Modes
-Rush (Start waves on 2 minute timer, instead of 30 seconds after last wave)
-Tech (No tower wave restrictions)
-Endless (Rounds = Inf)

 (Other mods require multiplayer to make sense

Mods
-Rounds (Round where game optionally ends.)
-Lives (Lives you start with)

?
-Gold (Starting gold and growth rate?


	*/

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame()
	{
		SceneManager.LoadScene("Game");
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
		//SceneManager.UnloadSceneAsync (SceneManager.GetSceneByName ("Menu"));
	}


	public void Quit()
	{

		// save any game data here
		#if UNITY_EDITOR
		// Application.Quit does not work in the editor so
		// UnityEditor.EditoryApplication.isPlayer needs to be set to false to end the game
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif

	}

}
