using UnityEngine;
using System.Collections;
using Pathfinding;

public class Unit : Entity
{

    public Prototype Prototype { protected set; get; }
    


    public virtual void ApplyPrototype(Prototype iPrototype)
    {
        Prototype = iPrototype;
    }


    public virtual void Kill(Unit killingUnit)
    {
        onDeath(killingUnit);


        //print("Kill it");
        Destroy(this.gameObject);
    }

    public virtual void onDeath(Unit killingUnit)
    {

    }

    // Removal doesn't trigger any on-death effects.
    public void remove()
    {

        Destroy(this);
    }
    


    public virtual void OnMouseDown()
    {
        if (Input.GetKey("mouse 0"))
        {
            Player.Active.SelectUnit(this);
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