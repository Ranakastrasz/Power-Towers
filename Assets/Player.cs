using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Active = null;
    public Unit selectedUnit = null;

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
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelectUnit(Unit iSelectedUnit)
    {
        Active.selectedUnit = iSelectedUnit;
    }

    public void AddGold(int bounty)
    {
        Active.Gold += bounty;
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
