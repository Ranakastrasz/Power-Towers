using UnityEngine;
using System.Collections;
using Pathfinding;
using System;


public delegate void OnDeathEventHandler(Unit iUnit, Unit iSource);

public class Unit : Entity
{

    public static OnDeathEventHandler _onDeathEvent;
    /*
     * Kill a unit, removing it, but also running OnDeath effects.
    */
    //public event OnDeath onDeath;

    // Use this for initialization

    public static void Init()
    {
        _onDeathEvent += new OnDeathEventHandler(onDeathEvent);
    }

    protected override void Start()
    {
        base.Start();
    }


    public virtual void Kill(Unit killingUnit)
    {

        _onDeathEvent(this, killingUnit);

        //print("Kill it");
        Destroy(this.gameObject);
    }

    public static void onDeathEvent(Unit iUnit, Unit iSource)
    {
        iUnit.onDeath(iSource);
    }

    public virtual void onDeath(Unit iSource)
    {
		remove ();
    }

    public virtual void OnMouseDown()
    {
        if (Input.GetKey("mouse 0"))
        {
            InputManager.ClickUnit(this);
            //Player.Active.SelectUnit(this);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }


    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    public virtual void FixedUpdate()
    {
        // Nothing Special for units unless clicked
    }
    
}