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
    
    public ProjectilePrototype _prototype { protected set; get; }

    public Unit _originUnit { protected set; get; }
    public float _speed { protected set; get; }
    public Entity _targetEntity { protected set; get; }
    public Vector3 _targetPoint { protected set; get; }



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
		_originUnit = iOriginUnit;
		_targetEntity = iTargetEntity;
		_targetPoint = _targetEntity.transform.position;

		SpriteRenderer Sprite = this.GetComponent<SpriteRenderer>();
		Sprite.sprite = _prototype._sprite;

    }
    public void Init(ProjectilePrototype iPrototype, Unit iOriginUnit, Vector3 iTargetPoint)
    {
        ApplyPrototype(iPrototype);
		_originUnit = iOriginUnit;
		_targetEntity = null;
		_targetPoint = iTargetPoint;

		SpriteRenderer Sprite = this.GetComponent<SpriteRenderer>();
		Sprite.sprite = _prototype._sprite;
        
    }
	

    public  void ApplyPrototype(ProjectilePrototype iPrototype)
    {
        _prototype = iPrototype;
    }
   

    // On Impact, call Prototype.Payload(this, SourceUnit, ImpactUnit, ImpactPoint)

	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
        

        if (_targetEntity != null)
		{
			_targetPoint = _targetEntity.transform.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, _targetPoint, _prototype._speed * Time.deltaTime);

        Vector3 distance = _targetPoint - transform.position;
        
        if (_targetEntity == null && distance.magnitude < 0.1)
        {
            _prototype._effect.ApplyPoint(_originUnit, this, _targetPoint);
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
            _prototype._effect.ApplyEntity(_originUnit, this, entity);
        }
        
    }
}
