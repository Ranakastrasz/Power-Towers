using UnityEngine;
using System.Collections;


public class Creep : Unit
{
    public GameObject Container;

    public int Life { protected set; get; }
    public int Lifez { protected set; get; }
    public int MaxLife { protected set; get; }

    public int Bounty { protected set; get; }
    

    public float BASE_SPEED = 3; // Figure out how to put this in unity editor instead.
    private AILerp lerp;


    public static ArrayList creepList = new ArrayList();

    public static void RefreshPathing()
    {
        foreach (Creep creep in creepList)
        {
            creep.lerp.SearchPath();
        }
    }

    public static Creep Create()
    {
        return null;
    }

    public void Init(int iHitpoints, int iBounty, Transform targetPoint)
    {
        
        gameObject.GetComponent<AILerp>().target = targetPoint;

        MaxLife = iHitpoints;
        Life = iHitpoints;
        Bounty = iBounty;


    }

    new private void Start()
    {
        base.Start();
        creepList.Add(this);

        //maxLife = 1;
        //life = 1;
        lerp = gameObject.GetComponent<AILerp>();

        lerp.speed = BASE_SPEED;

        lerp.ForceSearchPath();

    }

    protected void OnDestroy()
    {
        creepList.Remove(this);
    }

    public void Damage(Unit source, int amount)
    {
        Life -= amount;
        // Deduct life equal to damage dealth
        // If it hits zero or passes it, its lethal.
        if (Life <= 0)
        {
            this.Kill(source);
        }
    }

    public override void Kill(Unit killingUnit)
    {
        base.Kill(killingUnit);

        if (killingUnit is Tower)
        {
            Tower tower = killingUnit as Tower;
            tower.KillCredit(this);
            Player.Active.AddGold(this.Bounty);
        }

    }

    public override void Redraw()
    {
        float Red = ((float)Life /MaxLife);
        float Green =  1 - ((float)Life/MaxLife);
        float Blue = 0.5f;
        float Alpha = 1 ;

        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        sprite.material.SetColor("_Color", new Color(Red, Green, Blue,Alpha));

    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // Graphical nonsense.
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();
        if (Input.GetKey("mouse 0"))
        {
            // Damage on Click. Disabled
           // Damage(this, 1);
        }
    }
    
}