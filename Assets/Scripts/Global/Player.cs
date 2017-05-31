using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {


    public static Player Active = null;
    public Unit _selectedUnit { protected set; get; }

    // Change to selected Units later??

    public GameObject CircleUIPrefab;

    public int _gold { protected set; get; }
    public int _lives { protected set; get; }
    



    static public Unit SelectedUnit
    {
        get { return Active._selectedUnit; }
    }



    // Use this for initialization
    void Start ()
    {
        Active = this;
        _gold = 25;
        _lives = 25;
        _selectedUnit = null;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    

    public GameObject AddSelectionCircle(Unit iUnit, float iDiameter, Color? iColor = null)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Active.CircleUIPrefab, iUnit.transform.position, Quaternion.identity);



        obj.transform.SetParent(iUnit.transform);
        // Contains the whole circle, so size is diameter.
        obj.transform.localScale = new Vector3(iDiameter, iDiameter, iDiameter);

        Material rendererMaterial = obj.GetComponent<MeshRenderer>().material;
        rendererMaterial.color = iColor ?? Color.black;

        // Thickness is in both directions from the perfect radius
        // At 0.5, is half of radius itself.
        // Thickness should be a constant.. Thinking 0.05 Which is 0.05 / scale, or iRadus

        // Radius is iRadus - thickness/2
        // No idea why this stuff didn't work, had to get the "Sorting View" External plugin to fix the stupid thing.
        //obj.GetComponent<MeshRenderer>().sortingLayerID = SortingLayer.NameToID("Tower");// = "Default";// = SortingLayer.GetLayerValueFromID(6);
        //obj.GetComponent<MeshRenderer>().sortingOrder = 1;// = "Default";// = SortingLayer.GetLayerValueFromID(6);
        rendererMaterial.SetFloat("_Thickness", 0.02f/ iDiameter);
        rendererMaterial.SetFloat("_Radius", 0.5f - (0.01f / iDiameter));//0.5f-(0.02f/iRadius));
        // Beautiful!
        // /Render

        return obj;
    }

    // Add to a "Selection Manager" class
    public void SelectUnit(Unit iSelectedUnit)
    {
        Active._selectedUnit = iSelectedUnit;
        

        GameObject[] gameObjects =  GameObject.FindGameObjectsWithTag ("CircleUI");
 

        for(var i = 0 ; i < gameObjects.Length ; i ++)
            Destroy(gameObjects[i]);

        if (iSelectedUnit != null)
        {

            Tower tower = iSelectedUnit.GetComponent<Tower>();
            if (tower != null)
            {
                // Towers use Radius 1.4, because sqrt(2), almost covers the corners, but not quite. 
                AddSelectionCircle(iSelectedUnit, 1.4f, Color.green);
                if (tower._powerManager._prototype._canSend)
                {// Only if it can Transfer
                    // Ranges are in Radius, but circles are drawn via Diameter.
                    AddSelectionCircle(iSelectedUnit, PowerLink.RANGE_SHORT*2, Color.blue);
                }
                if (tower._powerManager._prototype._canSendLong)
                {// Over Long Distances.
                    AddSelectionCircle(iSelectedUnit, PowerLink.RANGE_LONG * 2, Color.blue);
                }
                if (tower._attackManager._prototype._range != 0.0f)
                { // Only show attack range if it has an Attack.
                    AddSelectionCircle(iSelectedUnit, tower._attackManager._prototype._range * 2, Color.red);
                }
            }
            else
            {
                // Creeps use radius 1 selector.
                AddSelectionCircle(iSelectedUnit, 1.0f, Color.red);
            }
        }
        else
        {
            
        }
    }

    public void AddGold(int iGold)
    {
        Active._gold += iGold;
    }

	public void DrainLife(int iLife)
	{
		_lives -= iLife;
		if (_lives <= 0)
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

		SceneManager.LoadScene("Menu");
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
		//SceneManager.UnloadSceneAsync (SceneManager.GetSceneByName ("Menu"));

	}

    public bool SpendGold(int price)
    {
        if (Active._gold >= price)
        {
            Active._gold -= price;
            return true;
        }
        else
        {
            return false;
        }
    }
}
