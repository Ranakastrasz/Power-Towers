using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCard : MonoBehaviour {

    
	public Text TextUpgrade;
	public Text TextSell;
	public Text TextShortLink;
	public Text TextLongLink;
	public Text TextRemoveLink;
    
    private Color COLOR_NORMAL = new Color(0.25f, 0.25f, 0.25f);
    private Color COLOR_SELECTED = new Color(0f, 0.0f, 1.0f);
    private Color COLOR_NOT_SELECTED = new Color(0.5f, 0.5f, 0.5f);

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q")) // Move to KeyManager via Hotkey class.
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
        Redraw();
    }
    
    private void Clear()
    {
        // Ok, this is not going to fly anymore.
        // Instead of Clear and Redraw, I need to only change what is needed.
        // Clear and redraw is two IO operations always, instead of one operation only when needed.
        // Could start by taking clear out and using else statements.
        // Ideally I would record last tick's data, and only then change if it changes.

        //TextUpgrade.color = COLOR_NOT_SELECTED;
        //
        ////TextSell.color = COLOR_NOT_SELECTED; Never not visible so far as I can think
        //
        //
        //TextShortLink.color = COLOR_NOT_SELECTED;
        //TextLongLink.color = COLOR_NOT_SELECTED;
        //TextRemoveLink.color = COLOR_NOT_SELECTED;
        //
        //TextUpgrade.text = "----";
        //TextSell.text = "----";
        //TextShortLink.text = "----";
        //TextLongLink.text = "----";
        //TextRemoveLink.text = "----";


    }

    public void Redraw()
    {

        if (Player.SelectedUnit != null && Player.SelectedUnit is Tower)
        {
            Tower tower = (Tower)Player.SelectedUnit;
            PowerManager power = tower._powerManager;
            // Needs history data still.
            // Currently, it still will call IO even if it doesn't need to, with the value being unchanged. Hence wasted effort.
            // first, check how expensive set and get are
            // Then experiment to see how expensive it is to just store last tick's data, to compare with.
            {
                // Highlight the buttons corresponding to which ability is active.
                bool shortSelected = (InputManager.Active.MouseState == InputManager.MOUSE_STATE.ADD_SHORT_LINK);
                bool longSelected = (InputManager.Active.MouseState == InputManager.MOUSE_STATE.ADD_LONG_LINK);
                bool removeSelected = (InputManager.Active.MouseState == InputManager.MOUSE_STATE.REMOVE_LINK);

                if (shortSelected || longSelected || removeSelected)
                {
                    if (shortSelected) TextShortLink.color = COLOR_SELECTED; else TextShortLink.color = COLOR_NOT_SELECTED;
                    if (longSelected) TextLongLink.color = COLOR_SELECTED; else TextLongLink.color = COLOR_NOT_SELECTED;
                    if (removeSelected) TextRemoveLink.color = COLOR_SELECTED; else TextRemoveLink.color = COLOR_NOT_SELECTED;
                }
                else
                {
                    TextShortLink.color = COLOR_NORMAL;
                    TextLongLink.color = COLOR_NORMAL;
                    TextRemoveLink.color = COLOR_NORMAL;
                }
            }

            if (tower._prototype._upgradesTo != null)
            {
                int price = tower._prototype._upgradesTo._price - tower._prototype._price;
                TextUpgrade.text = "Upgrade (" + price + ") (Q)";

                if (Player.Active._gold >= price)
                {
                    TextUpgrade.color = COLOR_NORMAL;
                }
                else
                {
                    TextUpgrade.color = COLOR_NOT_SELECTED;
                }

            }
            else
            {
                TextUpgrade.text = "----";
            }

            TextSell.text = "Sell (" + tower._prototype._price + ") (W)";
            TextSell.color = COLOR_NORMAL;

            if (true) // Is not consumer.
            {
                TextShortLink.text = "Short Link (A)";
            }
            if (true) // Is Transfer Tower.
            {
                TextLongLink.text = "Long Link (S)";
            }
            if (true) // Is not Consumer
            {
                TextRemoveLink.text = "Remove Link (D)";
            }

        }
        else
        {
            TextUpgrade.color = COLOR_NOT_SELECTED;
            
            TextSell.color = COLOR_NOT_SELECTED;
            
            TextShortLink.color = COLOR_NOT_SELECTED;
            TextLongLink.color = COLOR_NOT_SELECTED;
            TextRemoveLink.color = COLOR_NOT_SELECTED;
            
            TextUpgrade.text = "----";
            TextSell.text = "----";
            TextShortLink.text = "----";
            TextLongLink.text = "----";
            TextRemoveLink.text = "----";
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
