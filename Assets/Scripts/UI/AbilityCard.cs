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
        Clear();
        if (Player.SelectedUnit != null)
        {
            Redraw();
        }
        else
        {
        }
    }
    
    private void Clear()
    {
    
        TextUpgrade.color = COLOR_NOT_SELECTED;

        //TextSell.color = COLOR_NOT_SELECTED; Never not visible so far as I can think


        TextShortLink.color = COLOR_NOT_SELECTED;
        TextLongLink.color = COLOR_NOT_SELECTED;
        TextRemoveLink.color = COLOR_NOT_SELECTED;

        TextUpgrade.text = "----";
        TextSell.text = "----";
        TextShortLink.text = "----";
        TextLongLink.text = "----";
        TextRemoveLink.text = "----";


    }

    public void Redraw()
    {

        if (Player.SelectedUnit is Tower)
        {
            Tower tower = (Tower)Player.SelectedUnit;
            PowerManager power = tower.PowerManager;

            if (InputManager.Active.MouseState == InputManager.MOUSE_STATE.ADD_SHORT_LINK)
            {
                TextShortLink.color = COLOR_SELECTED;
            }
            else if (InputManager.Active.MouseState == InputManager.MOUSE_STATE.ADD_LONG_LINK)
            {
                TextLongLink.color = COLOR_SELECTED;
            }
            else if (InputManager.Active.MouseState == InputManager.MOUSE_STATE.REMOVE_LINK)
            {
                TextRemoveLink.color = COLOR_SELECTED;
            }
            else
            {
                TextShortLink.color = COLOR_NORMAL;
                TextLongLink.color = COLOR_NORMAL;
                TextRemoveLink.color = COLOR_NORMAL;
            }

            if (tower.Prototype.UpgradesTo != null)
            {
                int price = tower.Prototype.UpgradesTo.Price - tower.Prototype.Price;
                TextUpgrade.text = "Upgrade (" + price + ") (Q)";

                if (Player.Active.Gold >= price)
                {
                    TextUpgrade.color = COLOR_NORMAL;
                }

            }
            TextSell.text = "Sell (" + tower.Prototype.Price + ") (W)";

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
