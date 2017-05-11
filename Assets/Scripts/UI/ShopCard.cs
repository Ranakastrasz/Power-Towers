using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopCard : MonoBehaviour {

    public static ShopCard Active;

    public GameObject ButtonPrefab;
    public GameObject SpacerPrefab;

    private GameObject column1;
    private GameObject column2;

    public List<TowerPrototype> towerList { protected set; get; }

    // Use this for initialization
    void Start()
    {
        Active = this;
        towerList = new List<TowerPrototype>();
		towerList.Add(PrototypeDatabase.Active.Cannon[0]);
		towerList.Add(PrototypeDatabase.Active.Poison[0]);
		towerList.Add(PrototypeDatabase.Active.Lightning[0]);
		towerList.Add(PrototypeDatabase.Active.Flame[0]);

        towerList.Add(PrototypeDatabase.Active.Generator[0]);
        towerList.Add(PrototypeDatabase.Active.Transfer[0]);
        towerList.Add(PrototypeDatabase.Active.Wall);

        /*
         
            -1, t - cannon
            -2, y - lightning
            -3, u - frost
            -4, i - holy
            -5, o - demon
            -6, g - poison
            -7, h - weaken
            -8, j - generator
            -9, k - transfer
            -0, l - wall
         */

        column1 = transform.GetChild(0).gameObject;
        column2 = transform.GetChild(1).gameObject;

        for (int z = 0; z < 12; z++)
        {
            GameObject currentColumn = (z % 2)==0 ? column1 : column2;
            if (z < towerList.Count)
            {
                AddButton(currentColumn, towerList[z], z);
            }
            else
            {
                AddSpacer(currentColumn);// AddButton(currentColumn, null);
            }
        }


    }

    private GameObject AddSpacer(GameObject iColumn)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(SpacerPrefab, iColumn.transform.position, Quaternion.identity);
        obj.transform.SetParent(iColumn.transform, false);
        return obj;
    }

    private GameObject AddButton(GameObject iColumn, TowerPrototype iPrototype, int index)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(ButtonPrefab, iColumn.transform.position, Quaternion.identity);
        obj.transform.SetParent(iColumn.transform,false);
        GameObject text = obj.transform.GetChild(0).gameObject;
        if (iPrototype != null)
        {
            text.GetComponent<Text>().text = iPrototype.Name + ": " + iPrototype.Price + "G, ("+(index+1)+")";
            obj.GetComponent<Button>().onClick.AddListener(() => Button_Pressed(iPrototype));

        }
        else
        {
            text.GetComponent<Text>().text = "_";
            obj.SetActive(false);
        }
        return obj;

    }

    public void Button_Pressed(TowerPrototype iPrototype)
    {
        InputManager.SelectTower(iPrototype);
        //InputManager.ToggleMouseState(InputManager.MOUSE_STATE.PLACE_TOWER);
    }
}
