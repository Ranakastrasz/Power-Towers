using UnityEngine;
using RanaLib;
using Pathfinding;
using System.Collections;
using System;

public class Tower : Unit
{
    public AttackManager AttackManager { protected set; get; }
    public PowerManager PowerManager { protected set; get; }

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

    public void Init(Prototype iPrototype)
    {

        AttackManager = this.gameObject.AddComponent<AttackManager>();
        PowerManager = this.gameObject.AddComponent<PowerManager>();
        
        UpdatePathing();

        ApplyPrototype(iPrototype);


    }

    public override void ApplyPrototype(Prototype iPrototype)
    {
        base.ApplyPrototype(iPrototype);
        TowerPrototype towerPrototype = Prototype as TowerPrototype;
        AttackManagerPrototype attackManagerPrototype = towerPrototype.AttackManagerPrototype;
        PowerManagerPrototype powerManagerPrototype = towerPrototype.PowerManagerPrototype;

        gameObject.name = towerPrototype.Name;


        AttackManager.ApplyPrototype(attackManagerPrototype);
        PowerManager.ApplyPrototype(powerManagerPrototype);
        
    }


    public void Sell()
    {
        Player.Active.AddGold((this.Prototype as TowerPrototype).Price);
        Kill(this);
    }

    public override void Kill(Unit killingUnit)
    {

        UpdatePathing();

        base.Kill(killingUnit);
    }
    

    internal void KillCredit(Creep creep)
    {
        // Add Killcount, and apply onkill abilities if applicable. No idea what this might do tho.
    }
    
    private void UpdatePathing()
    {

        GraphUpdateObject guo = new GraphUpdateObject(GetComponent<Collider>().bounds);
        
        AstarPath.active.UpdateGraphs(guo, 0.1f);
        //Pathfinding.Console.Write ("// Flushing\n");
        AstarPath.active.FlushGraphUpdates();

        Creep.RefreshPathing();
    }


    public override void Redraw()
    {
        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        float Red   = 1.0f;
        float Green = 1.0f;
        float Blue  = 1.0f;
        float Alpha = 1.0f;
        if (PowerManager != null)
        {
            float Ratio = (float)PowerManager.Energy / PowerManager.Prototype.EnergyCap;

            Red = 0.2f + (0.8f * Ratio); // Scale from 20% light to 100% light based on current energy.
            Green = 0.2f + (0.8f * Ratio);
            Blue = 0.2f + (0.8f * Ratio);
            Alpha = 1;
        }

        sprite.material.SetColor("_Color", new Color(Red, Green, Blue, Alpha));

    }
}