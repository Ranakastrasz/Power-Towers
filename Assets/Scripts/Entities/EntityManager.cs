using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {


    public static EntityManager Active;
    
    public GameObject UnitContainer;

    public GameObject TowerPrefab;
    public GameObject TowerContainer;

    public GameObject RunnerPrefab;
    public GameObject RunnerContainer;
    
    public GameObject PowerLinkPrefab;
    public GameObject PowerLinkContainer;
    
    public GameObject ProjectilePrefab;
    public GameObject ProjectileContainer;

    public GameObject FloatingTextPrefab;
    public GameObject FloatingTextContainer;
    

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

    public static GameObject CreateRunner(Vector3 iPosition, int iHitpoints, int iBounty, Transform iTargetPosition, int iRound)
    {

        GameObject obj = (GameObject)GameObject.Instantiate(Active.RunnerPrefab, iPosition, Quaternion.identity);
        Runner runner = obj.GetComponent<Runner>();
        runner.transform.parent = Active.RunnerContainer.transform;

        runner.Init((int)iHitpoints, iBounty, iTargetPosition, iRound);

        return obj;
    }

	public static GameObject CreatePowerLink(PowerManager iSource, PowerManager iTarget)
	{

		GameObject obj = (GameObject)GameObject.Instantiate(Active.PowerLinkPrefab, iSource.gameObject.transform.position, Quaternion.identity);
		PowerLink powerLink = obj.GetComponent<PowerLink>();
		powerLink.transform.parent = Active.PowerLinkContainer.transform;
		powerLink.Init(iSource, iTarget);

		iSource._powerLinksOut.Add(powerLink);
		iTarget._powerLinksIn.Add(powerLink);

		return obj;
	}

    public static GameObject CreateFloatingText(Vector3 iPosition, string iText, float iDuration, Color? iColor = null)
    {

		GameObject obj = (GameObject)GameObject.Instantiate(Active.FloatingTextPrefab, iPosition, Quaternion.identity);
		FloatingText floatingText = obj.GetComponent<FloatingText>();

		floatingText.transform.parent = Active.FloatingTextContainer.transform;

		floatingText.Init(iText, iDuration, iColor ?? Color.black);
        

		return obj;
    }

    /*public static Unit[] GetUnits()
    {
        Unit[] units = EntityManager.Active.UnitContainer.GetComponentsInChildren<Unit>(); // Wrong, won't work for all units. yet
        return units;
    }*/
    public static Runner[] GetRunners()
    {
        Runner[] runners = EntityManager.Active.RunnerContainer.GetComponentsInChildren<Runner>();
        return runners;
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
