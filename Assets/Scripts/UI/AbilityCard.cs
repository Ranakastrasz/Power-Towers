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
        InputManager.SetMouseState(InputManager.MOUSE_STATE.ADD_SHORT_LINK);
    }
    public void Button_LongLink()
    {
        InputManager.SetMouseState(InputManager.MOUSE_STATE.ADD_LONG_LINK);
    }
    public void Button_RemoveLink()
    {
        InputManager.SetMouseState(InputManager.MOUSE_STATE.REMOVE_LINK);
    }
}
