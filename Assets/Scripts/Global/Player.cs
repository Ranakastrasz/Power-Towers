﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    public static Player Active = null;
    public Unit selectedUnit { protected set; get; }

    // Change to selected Units later??

    public GameObject SelectionIndicatorPrefab;

    public int Gold { protected set; get; }
    public int Lives { protected set; get; }
    



    static public Unit SelectedUnit
    {
        get { return Active.selectedUnit; }
    }



    // Use this for initialization
    void Start ()
    {
        Active = this;
        Gold = 25;
        Lives = 25;
        selectedUnit = null;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    

    public void SelectUnit(Unit iSelectedUnit)
    {
        Active.selectedUnit = iSelectedUnit;
        
        GameObject[] gameObjects =  GameObject.FindGameObjectsWithTag ("SelectionIndicator");
 
        for(var i = 0 ; i < gameObjects.Length ; i ++)
            Destroy(gameObjects[i]);

        if (iSelectedUnit != null)
        {
            GameObject obj = (GameObject)GameObject.Instantiate(Active.SelectionIndicatorPrefab, iSelectedUnit.transform.position, Quaternion.identity);

            obj.transform.SetParent(iSelectedUnit.transform);
            if (iSelectedUnit.GetComponent<Tower>())
            {
                obj.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            }
            else
            { }
        }
        else
        {
            
        }
    }

    public void AddGold(int iGold)
    {
        Active.Gold += iGold;
    }

	public void DrainLife(int iLife)
	{
		Lives -= iLife;
		if (Lives <= 0)
		{
			// GameOver!
			// How to handle?
			// need a gamestate class, sort of
			// And have it force the game into a pause state, I think.
			Invoke("Quit",3.0f);
			Timer.SetGameSpeed (Timer.TIME_SCALE.FULL);
		}

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

    public bool SpendGold(int price)
    {
        if (Active.Gold >= price)
        {
            Active.Gold -= price;
            return true;
        }
        else
        {
            return false;
        }
    }
}
