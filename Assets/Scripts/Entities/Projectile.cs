using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    // Projectiles have a Target Unit.
    // Maybe a Target point.
    // Or just an Angle it is locked into if neither is set, for Wave tower.
    // A Payload (Temperary, Deal damage and die as hardcoded)
    // Has a speed to move.
    // Turning speed and whatnot is ignored, projectiles never miss.
    //
    
    public ProjectilePrototype Prototype { protected set; get; }

    public Unit OriginUnit { protected set; get; }
    public float Speed { protected set; get; }
    public Entity TargetEntity { protected set; get; }
    public Vector3 TargetPoint { protected set; get; }



	// Use this for initialization
	protected override void Start ()
    {
		
	}

    // 
	// Guess is that it targeted a unit the same tick it was destroyed, so was created with a target,
	// But the target stopped existing.


    public void Init(ProjectilePrototype iPrototype, Unit iOriginUnit, Entity iTargetEntity)
    {
        ApplyPrototype(iPrototype);
		OriginUnit = iOriginUnit;
		TargetEntity = iTargetEntity;
		TargetPoint = TargetEntity.transform.position;

		SpriteRenderer Sprite = this.GetComponent<SpriteRenderer>();
		Sprite.sprite = Prototype.Sprite;

    }
    public void Init(ProjectilePrototype iPrototype, Unit iOriginUnit, Vector3 iTargetPoint)
    {
        ApplyPrototype(iPrototype);
		OriginUnit = iOriginUnit;
		TargetEntity = null;
		TargetPoint = iTargetPoint;

		SpriteRenderer Sprite = this.GetComponent<SpriteRenderer>();
		Sprite.sprite = Prototype.Sprite;
        
    }
	

    public  void ApplyPrototype(ProjectilePrototype iPrototype)
    {
        Prototype = iPrototype;
    }
   

    // On Impact, call Prototype.Payload(this, SourceUnit, ImpactUnit, ImpactPoint)

	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
        

        if (TargetEntity != null)
		{
			TargetPoint = TargetEntity.transform.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, TargetPoint, Prototype.Speed * Time.deltaTime);

        Vector3 distance = TargetPoint - transform.position;
        
        if (TargetEntity == null && distance.magnitude < 0.1)
        {
            Prototype.Effect.ApplyPoint(OriginUnit, this, TargetPoint);
        }

		float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

    
    void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.GetComponent<Entity>() != null)
        {

            Entity entity = gameObject.GetComponent<Entity>();
            Prototype.Effect.ApplyEntity(OriginUnit, this, entity);
        }
        
    }
}
