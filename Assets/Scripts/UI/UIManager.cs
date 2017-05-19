using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;
public class UIManager : MonoBehaviour {

    public static UIManager Active { protected set; get; }
    public UnitPanel _unitPanel;

	public Text Gold;
	public Text Life;
   

    static public UnitPanel UnitPanel
    {
        get { return Active._unitPanel; }
    }

	public static String formatFloat(float iFloat, int places = 2)
    {
        return "" + Math.Round(iFloat, places);
    }

    // Use this for initialization
    void Start ()
    {
        if (Active == null)
        {
            Active = this;
        }
        else
        {
        }
    }

    public static void UpgradeTower()
    {
        if (Player.SelectedUnit && Player.SelectedUnit is Tower)
        {
            Tower selectedTower = Player.SelectedUnit as Tower;
            TowerPrototype current = selectedTower._prototype as TowerPrototype;
            TowerPrototype upgradeTo = current._upgradesTo;
            if (upgradeTo != null)
            {
                int price = upgradeTo._price - current._price;
                if (Player.Active.SpendGold(price))
                {
                    selectedTower.ApplyPrototype(upgradeTo);
                }
                else
                {
                    EntityManager.CreateFloatingText(selectedTower.transform.position, "Not Enough Resources", 1.0f, InputManager.TEXT_INSUFFICIENT_RESOURCES);

                    //print("Not Enough Gold");
                }
            }
        }
    }
    public static void SellTower()
    {
        if (Player.SelectedUnit && Player.SelectedUnit is Tower)
        {
            Tower selectedTower = Player.SelectedUnit as Tower;
            selectedTower.Sell();
        }
    }

    // Update is called once per frame
	void Update ()
	{
		Gold.text = "Res : "+Player.Active._gold;
		Life.text = "Life: "+Player.Active._lives;
    }

	void RenderGrid()
	{
		/**/

	}

    

}
