using UnityEngine;
using System.Collections;
using Pathfinding;
using UnityEngine.Events;

/// <summary>  
/// A Unit which represents an Enemy, able to move, take damage, and die.
/// </summary>  
/// 

public delegate void OnDamageEventHandler(Unit iSource, int iDamage);

public class Runner : Unit
{


    public event OnDamageEventHandler _onDamageEvent;

    public const string MOVEMENT_SPEED = "movementSpeed";

    public int _life { protected set; get; }
    public int _maxLife { protected set; get; }

    public int _bounty { protected set; get; }

    private AILerp _lerp;

    private SpriteRenderer _healthSprite;
    private SpriteRenderer _shieldSprite;
    private SpriteRenderer _speedSprite;
    private SpriteRenderer _feedbackSprite;




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
        _lerp.speed = RunnerData.BASE_SPEED;
        gameObject.name = "Runner " + iRound;

        _maxLife = iHitpoints;
        _life = iHitpoints;
        _bounty = iBounty;


    }
    protected override void RecalculateStats()
    {
        _lerp.speed = _statTable[MOVEMENT_SPEED].modifiedValue;
    }

    new private void Start()
    {
        _healthSprite = transform.Find("HealthNodeSprite").GetComponent<SpriteRenderer>();
        _shieldSprite = transform.Find("ShieldNodeSprite").GetComponent<SpriteRenderer>();
        _speedSprite = transform.Find("SpeedNodeSprite").GetComponent<SpriteRenderer>();
        _feedbackSprite = transform.Find("FeedbackNodeSprite").GetComponent<SpriteRenderer>();

        base.Start();

        _statTable.Add(MOVEMENT_SPEED, new MutableStat(RunnerData.BASE_SPEED));

        _lerp.SearchPath();

        _shieldSprite.material.SetColor("_Color", new Color(UnityEngine.Random.Range(0.5f, 1f), UnityEngine.Random.Range(0.5f, 1f), 1, 1f));
        _speedSprite.material.SetColor("_Color", new Color(1, UnityEngine.Random.Range(0.5f, 1f), UnityEngine.Random.Range(0.5f, 1f), 1f));
        _feedbackSprite.material.SetColor("_Color", new Color(UnityEngine.Random.Range(0.5f, 1f), 1, UnityEngine.Random.Range(0.5f, 1f), 1f));

        gameObject.AddComponent<Feedback>();

    }

    protected void OnDestroy()
    {
    }

    public void Damage(Unit iSource, int iLife)
    {
        if (_life > 0)
        {
            _onDamageEvent(iSource, iLife);
            _life -= iLife;
            // Deduct life equal to damage dealth
            // If it hits zero or passes it, its lethal.
            if (_life <= 0)
            {
                this.Kill(iSource);
            }
        }
    }

    /*public override void Kill(Unit killingUnit)
    {
        base.Kill(killingUnit);

    }*/


    public override void Redraw()
    {
        float Red = 1 - ((float)_life / _maxLife);
        float Green = ((float)_life / _maxLife);
        float Blue = 0f;
        float Alpha = 1;

        _healthSprite.material.SetColor("_Color", new Color(Red, Green, Blue, Alpha));

    }

    public override void onDeath(Unit iSource)
    {
        if (iSource is Tower)
        {
            Tower tower = iSource as Tower;
            tower.KillCredit(this);
            Player.Active.AddGold(this._bounty);
        }
    }



}