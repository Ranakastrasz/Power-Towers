using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {


    public static EntityManager Active;

    public GameObject TowerPrefab;
    public GameObject TowerContainer;

    public GameObject CreepPrefab;
    public GameObject CreepContainer;

    public static GameObject CreateTower(Vector3 iPosition, TowerPrototype iPrototype)
    {

        GameObject obj = (GameObject)GameObject.Instantiate(Active.TowerPrefab, iPosition, Quaternion.identity);
        Tower tower = obj.GetComponent<Tower>();

        obj.transform.parent = Active.TowerContainer.transform;

        tower.Init(iPrototype);

        return obj;
    }

    public static GameObject CreateCreep(Vector3 iPosition, int hitpoints, Transform iTargetPosition)
    {

        GameObject obj = (GameObject)GameObject.Instantiate(Active.CreepPrefab, iPosition, Quaternion.identity);
        Creep creep = obj.GetComponent<Creep>();
        creep.transform.parent = Active.CreepContainer.transform;
        
        creep.Init((int)hitpoints, 1, iTargetPosition);

        return obj;
    }

    public static Creep[] GetCreeps()
    {
        Creep[] creeps = EntityManager.Active.CreepContainer.GetComponentsInChildren<Creep>();
        return creeps;
    }
    public static Tower[] GetTowers()
    {
        Tower[] towers = EntityManager.Active.TowerContainer.GetComponentsInChildren<Tower>();
        return towers;
    }

    // Use this for initialization
    void Start ()
    {
        Active = this;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
