using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {


    public static EntityManager Active;
    
    public GameObject UnitContainer;

    public GameObject TowerPrefab;
    public GameObject TowerContainer;

    public GameObject CreepPrefab;
    public GameObject CreepContainer;
    
    public GameObject PowerLinkPrefab;
    public GameObject PowerLinkContainer;

    public GameObject ProjectilePrefab;
    public GameObject ProjectileContainer;

    public static GameObject CreateTower(Vector3 iPosition, TowerPrototype iPrototype)
    {

        GameObject obj = (GameObject)GameObject.Instantiate(Active.TowerPrefab, iPosition, Quaternion.identity);
        Tower tower = obj.GetComponent<Tower>();

        obj.transform.parent = Active.TowerContainer.transform;

        tower.Init(iPrototype);

        return obj;
    }

    internal static GameObject CreateProjectile(Unit iOrigin, Vector3 iPosition, Entity iTarget, ProjectilePrototype iPrototype)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Active.ProjectilePrefab, iPosition, Quaternion.identity);
        Projectile projectile = obj.GetComponent<Projectile>();

        obj.transform.parent = Active.TowerContainer.transform;

        projectile.Init(iPrototype, iOrigin, iTarget);

        return obj;
    }

    public static GameObject CreateCreep(Vector3 iPosition, int iHitpoints, int iBounty, Transform iTargetPosition)
    {

        GameObject obj = (GameObject)GameObject.Instantiate(Active.CreepPrefab, iPosition, Quaternion.identity);
        Creep creep = obj.GetComponent<Creep>();
        creep.transform.parent = Active.CreepContainer.transform;

        creep.Init((int)iHitpoints, iBounty, iTargetPosition);

        return obj;
    }

    public static GameObject CreatePowerLink(PowerManager iSource, PowerManager iTarget)
    {

        GameObject obj = (GameObject)GameObject.Instantiate(Active.PowerLinkPrefab, iSource.gameObject.transform.position, Quaternion.identity);
        PowerLink powerLink = obj.GetComponent<PowerLink>();
        powerLink.transform.parent = Active.PowerLinkContainer.transform;

        powerLink.Init(iSource, iTarget);
        
        iSource.PowerLinksOut.Add(powerLink);
        iTarget.PowerLinksIn.Add(powerLink);

        return obj;
    }
    
    /*public static Unit[] GetUnits()
    {
        Unit[] units = EntityManager.Active.UnitContainer.GetComponentsInChildren<Unit>(); // Wrong, won't work for all units. yet
        return units;
    }*/
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
    public static PowerLink[] GetPowerLinks()
    {
        PowerLink[] powerLinks = EntityManager.Active.PowerLinkContainer.GetComponentsInChildren<PowerLink>();
        return powerLinks;
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
