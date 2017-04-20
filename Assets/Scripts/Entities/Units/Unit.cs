using UnityEngine;
using System.Collections;
using Pathfinding;

public class Unit : Entity
{
    
    /*
     * Kill a unit, removing it, but also running OnDeath effects.
    */
    public virtual void Kill(Unit killingUnit)
    {
        onDeath(killingUnit);
        onRemove();

        //print("Kill it");
        Destroy(this.gameObject);
    }

    protected virtual void onDeath(Unit killingUnit)
    {

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