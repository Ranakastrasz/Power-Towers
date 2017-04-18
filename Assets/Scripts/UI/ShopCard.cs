using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopCard : MonoBehaviour {

    public static ShopCard Active;

    public GameObject ButtonPrefab;
    public GameObject SpacerPrefab;

    public TowerPrototype SelectedTower { set; get; }

    private GameObject column1;
    private GameObject column2;

    private List<TowerPrototype> towerList = new List<TowerPrototype>();

    // Use this for initialization
    void Start()
    {
        Active = this;
        towerList.Add(PrototypeDatabase.Active.Wall);
        towerList.Add(PrototypeDatabase.Active.RockLauncher[0]);
        towerList.Add(PrototypeDatabase.Active.TeslaCoil[0]);
        towerList.Add(PrototypeDatabase.Active.Generator[0]);

        column1 = transform.GetChild(0).gameObject;
        column2 = transform.GetChild(1).gameObject;

        for (int z = 0; z < 12; z++)
        {
            GameObject currentColumn = (z % 2)==0 ? column1 : column2;
            if (z < towerList.Count)
            {
                AddButton(currentColumn, towerList[z]);
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

    private GameObject AddButton(GameObject iColumn, TowerPrototype iPrototype)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(ButtonPrefab, iColumn.transform.position, Quaternion.identity);
        obj.transform.SetParent(iColumn.transform,false);
        GameObject text = obj.transform.GetChild(0).gameObject;
        if (iPrototype != null)
        {
            text.GetComponent<Text>().text = iPrototype.Name + ":\n" + iPrototype.Price;
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
        SelectedTower = iPrototype;
    }
}
