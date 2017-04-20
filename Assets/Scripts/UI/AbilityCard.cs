using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCard : MonoBehaviour {
    

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("q"))
        {
            Button_Upgrade();
        }
        if (Input.GetKeyDown("w"))
        {
            Button_Sell();
        }
        if (Input.GetKeyDown("e"))
        {
            // Sellers Remorse
        }
        if (Input.GetKeyDown("a"))
        {
            Button_ShortLink();
        }
        if (Input.GetKeyDown("s"))
        {
            Button_LongLink();
        }
        if (Input.GetKeyDown("d"))
        {
            Button_RemoveLink();
        }
    }

    public void Button_Upgrade()
    {
        UIManager.UpgradeTower();
    }
    public void Button_Sell()
    {
        UIManager.SellTower();
    }
    public void Button_ShortLink()
    {
        InputManager.ToggleMouseState(InputManager.MOUSE_STATE.ADD_SHORT_LINK);
    }
    public void Button_LongLink()
    {
        InputManager.ToggleMouseState(InputManager.MOUSE_STATE.ADD_LONG_LINK);
    }
    public void Button_RemoveLink()
    {
        InputManager.ToggleMouseState(InputManager.MOUSE_STATE.REMOVE_LINK);
    }
}
