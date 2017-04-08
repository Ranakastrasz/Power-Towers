using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;
public class UIManager : MonoBehaviour {

    public static UIManager Active { protected set; get; }
    public UnitPanel unitPanel;

    public GameObject Database;

    static public UnitPanel UnitPanel
    {
        get { return Active.unitPanel; }
    }

    public static String formatFloat(float iFloat)
    {
        return "" + Math.Round(iFloat, 2);
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
            TowerPrototype current = selectedTower.Prototype as TowerPrototype;
            TowerPrototype upgradeTo = current.UpgradesTo;
            if (upgradeTo != null)
            {
                int price = upgradeTo.Price - current.Price;
                if (Player.Active.SpendGold(price))
                {
                    selectedTower.ApplyPrototype(upgradeTo);
                }
                else
                {
                    print("Not Enough Gold");
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

    }

    

}
