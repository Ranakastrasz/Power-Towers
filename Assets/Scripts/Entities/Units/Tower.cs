using UnityEngine;
using RanaLib;
using Pathfinding;
using System.Collections;
using System;

public class Tower : Unit
{
    public const string ATTACK_SPEED = "attackSpeed";
    public TowerPrototype _prototype { protected set; get; }
	public AttackManager _attackManager { protected set; get; }
    public PowerManager _powerManager { protected set; get; }
    public GameObject _turret { get; private set; }

    // Should autocleanup when you change it.
    // I think.


    new void Start()
    {
		base.Start();

		_statTable.Add(ATTACK_SPEED, new MutableStat(1f));
        
        // Locked to a 0.5 grid area.
        Vector3 p = transform.position;
        p.x = (float)RanaLib.Math.Round(p.x, 0.5);
        p.y = (float)RanaLib.Math.Round(p.y, 0.5);

		transform.position = new Vector3(p.x, p.y, p.z);




    }

    public void Init(TowerPrototype iPrototype)
    {
        _attackManager = this.gameObject.AddComponent<AttackManager>();
        _powerManager = this.gameObject.AddComponent<PowerManager>();
        
        _turret = gameObject.transform.FindChild("Turret").gameObject;


        UpdatePathing();
        
        ApplyPrototype(iPrototype);


    }

    public void ApplyPrototype(TowerPrototype iPrototype)
    {

		_prototype = iPrototype;
		AttackManagerPrototype attackManagerPrototype = _prototype._attackManagerPrototype;
		AbilityManagerPrototype abilityManagerPrototype = _prototype._abilityManagerPrototype;
        PowerManagerPrototype powerManagerPrototype = _prototype._powerManagerPrototype;

        gameObject.name = _prototype._name;


        _attackManager.ApplyPrototype(attackManagerPrototype,abilityManagerPrototype);
        _powerManager.ApplyPrototype(powerManagerPrototype);
        
        SpriteRenderer baseSprite = this.GetComponent<SpriteRenderer>();
        SpriteRenderer turretSprite = _turret.GetComponent<SpriteRenderer>();
        
        baseSprite.sprite = _prototype._baseSprite;
        turretSprite.sprite = _prototype._turretSprite;

    }
    

    public void Sell()
    {
        Player.Active.AddGold((this._prototype as TowerPrototype)._price);
        EntityManager.CreateFloatingText(transform.position, "+"+(this._prototype as TowerPrototype)._price.ToString(), 1.0f, InputManager.TEXT_INSUFFICIENT_RESOURCES);
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
        base.onRemove();
        _powerManager.RemoveLinks();
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

        
        if (_attackManager._currentTarget != null)
        {
            Vector3 relativePos = _attackManager._currentTarget.gameObject.transform.position - transform.position;

            float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;

            _turret.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


		// Would be cool, but I don't know enough yet.
		/*Buff buff = GetComponent<Buff> ();

		if (buff != null)
		{
			float angle = Turret.transform.rotation.eulerAngles.z;
			angle += (UnityEngine.Random.value-0.5f) * (Mathf.Deg2Rad) * 60.0f;
			Turret.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}*/


    }
}