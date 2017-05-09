using UnityEngine;
using RanaLib;
using Pathfinding;
using System.Collections;
using System;

public class Tower : Unit
{
	public TowerPrototype Prototype { protected set; get; }
	public AttackManager AttackManager { protected set; get; }
    public PowerManager PowerManager { protected set; get; }
    public GameObject Turret { get; private set; }

    // Should autocleanup when you change it.
    // I think.


    new void Start()
    {
        base.Start();
        
        // Locked to a 0.5 grid area.
        Vector3 p = transform.position;
        p.x = (float)RanaLib.Math.Round(p.x, 0.5);
        p.y = (float)RanaLib.Math.Round(p.y, 0.5);

        transform.position = new Vector3(p.x, p.y, p.z);



    }

    public void Init(TowerPrototype iPrototype)
    {
        AttackManager = this.gameObject.AddComponent<AttackManager>();
        PowerManager = this.gameObject.AddComponent<PowerManager>();
        
        Turret = gameObject.transform.FindChild("Turret").gameObject;

        UpdatePathing();
        
        ApplyPrototype(iPrototype);


    }

    public void ApplyPrototype(TowerPrototype iPrototype)
    {

		Prototype = iPrototype;
		AttackManagerPrototype attackManagerPrototype = Prototype.AttackManagerPrototype;
		AbilityManagerPrototype abilityManagerPrototype = Prototype.AbilityManagerPrototype;
        PowerManagerPrototype powerManagerPrototype = Prototype.PowerManagerPrototype;

        gameObject.name = Prototype.Name;


        AttackManager.ApplyPrototype(attackManagerPrototype,abilityManagerPrototype);
        PowerManager.ApplyPrototype(powerManagerPrototype);
        
        SpriteRenderer baseSprite = this.GetComponent<SpriteRenderer>();
        SpriteRenderer turretSprite = Turret.GetComponent<SpriteRenderer>();
        
        baseSprite.sprite = Prototype.BaseSprite;
        turretSprite.sprite = Prototype.TurretSprite;

    }
    

    public void Sell()
    {
        Player.Active.AddGold((this.Prototype as TowerPrototype).Price);
        Kill(this);
    }

    public override void Kill(Unit killingUnit)
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        UpdatePathing();

        base.Kill(killingUnit);
    }

    protected override void onRemove()
    {
        PowerManager.RemoveLinks();
    }


    internal void KillCredit(Runner runner)
    {
        // Add Killcount, and apply onkill abilities if applicable. No idea what this might do tho.
    }
    
    private void UpdatePathing()
    {

        GraphUpdateObject guo = new GraphUpdateObject(GetComponent<Collider>().bounds);
        
        AstarPath.active.UpdateGraphs(guo, 0.0f);
        //Pathfinding.Console.Write ("// Flushing\n");
        AstarPath.active.FlushGraphUpdates();
        
        Runner.RefreshPathing();
    }
    

    public override void Redraw()
    {
        //SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        /*float Red   = 1.0f;
        float Green = 1.0f;
        float Blue  = 1.0f;
        float Alpha = 1.0f;
        if (PowerManager.Prototype.MaxEnergy > 0)
        {
            float Ratio = (float)PowerManager.Energy / PowerManager.Prototype.MaxEnergy;

            Red = 0.2f + (0.8f * Ratio); // Scale from 20% light to 100% light based on current energy.
            Green = 0.2f + (0.8f * Ratio);
            Blue = 0.2f + (0.8f * Ratio);
            Alpha = 1;
        }

        sprite.material.SetColor("_Color", new Color(Red, Green, Blue, Alpha));*/

        
        if (AttackManager.CurrentTarget != null)
        {
            Vector3 relativePos = AttackManager.CurrentTarget.gameObject.transform.position - transform.position;

            float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
            Turret.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }



    }
}