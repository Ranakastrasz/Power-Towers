using UnityEngine;
using System.Collections;
using Pathfinding;

/// <summary>  
/// A Unit which represents an Enemy, able to move, take damage, and die.
/// </summary>  
public class Runner : Unit
{

    public int _life { protected set; get; }
    public int _maxLife { protected set; get; }

    public int _bounty { protected set; get; }
    

    public float BASE_SPEED = 3; // Figure out how to put this in unity editor instead.
    private AILerp _lerp;



    public static void RefreshPathing()
    {
        foreach (Runner runner in EntityManager.GetRunners())
        {
            //if (!runner.lerp.isPathValid())
            {
                runner.Invoke("SearchPath", 0.1f);
                //runner.lerp.SearchPath();
            }
        }
    }
    private void SearchPath()
    {
        _lerp.SearchPath();
    }

    public void Init(int iHitpoints, int iBounty, Transform targetPoint, int iRound)
    {

        _lerp = gameObject.GetComponent<AILerp>();

        _lerp.target = targetPoint;
        _lerp.speed = BASE_SPEED;
        gameObject.name = "Runner " + iRound;

        _maxLife = iHitpoints;
        _life = iHitpoints;
        _bounty = iBounty;
        
    }

    new private void Start()
    {
        base.Start();

        //lerp.ForceSearchPath();
        _lerp.SearchPath();

	    //Assumes a Seeker component is attached to the GameObject
	    //Seeker seeker = GetComponent<Seeker>();
	   
	    //seeker.pathCallback is a OnPathDelegate, we add the function OnPathComplete to it so it will be called whenever a path has finished calculating on that seeker
	    //seeker.pathCallback += OnPathComplete;
	   
    }

    protected void OnDestroy()
    {
    }

    public void Damage(Unit iSource, int iLife)
    {
		if (_life > 0)
		{
			_life -= iLife;
			// Deduct life equal to damage dealth
			// If it hits zero or passes it, its lethal.
			if (_life <= 0)
			{
				this.Kill (iSource);
			}
		}
    }

    public override void Kill(Unit killingUnit)
    {
        base.Kill(killingUnit);

        if (killingUnit is Tower)
        {
            Tower tower = killingUnit as Tower;
            tower.KillCredit(this);
            Player.Active.AddGold(this._bounty);
        }

    }

    public override void Redraw()
    {
        float Red = ((float)_life /_maxLife);
        float Green =  1 - ((float)_life/_maxLife);
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
    /*private void OnPathComplete(Path p)
    {
        this.remove();
	    Debug.Log ("This is called when a path is completed on the seeker attached to this GameObject");
    }*/
    
}