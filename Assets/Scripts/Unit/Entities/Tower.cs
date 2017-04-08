using UnityEngine;
using RanaLib;
using Pathfinding;
using System.Collections;
using System;

public class Tower : Unit
{
    public GameObject Container;
    // Can Attack creeps. Use some kind of trigger. !
    // Trigger on enter range, to get target? That might work, actually.
    //


    public PowerManager PowerManager { protected set; get; }

    new void Start()
    {
        base.Start();

        // Locked to a 0.5 grid area.
        Vector3 p = transform.position;
        p.x = (float)RanaLib.Math.Round(p.x, 0.5);
        p.y = (float)RanaLib.Math.Round(p.y, 0.5);

        transform.position = new Vector3(p.x, p.y, p.z);
        

        UpdatePathing();


    }

    public override void ApplyPrototype(Prototype iPrototype)
    {
        base.ApplyPrototype(iPrototype);
        Attack attack = this.gameObject.GetComponent<Attack>();
        TowerPrototype towerPrototype = Prototype as TowerPrototype;
        AttackPrototype attackPrototype = towerPrototype.Attack;
        PowerManagerPrototype powerManagerPrototype = towerPrototype.PowerManager;

        gameObject.name = towerPrototype.Name;

        if (attackPrototype == null )
        {
            if (attack == null)
            {
                // Do nothing
            }
            else
            {
                // Destroy
                Destroy(attack);
            }
        }
        else
        {
            if (attack == null)
            {
                // Create Prototype
                attack = this.gameObject.AddComponent<Attack>();
            }
            else
            {
                //already exists.
            }
            attack.ApplyPrototype(towerPrototype.Attack);
        }


        if (powerManagerPrototype == null)
        {
            if (PowerManager == null)
            {
                // Do nothing
            }
            else
            {
                // Destroy
                Destroy(PowerManager);
            }
        }
        else
        {
            if (PowerManager == null)
            {
                // Create Prototype
                PowerManager = this.gameObject.AddComponent<PowerManager>();
            }
            else
            {
                //already exists.
            }
            PowerManager.ApplyPrototype(towerPrototype.PowerManager);
        }

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