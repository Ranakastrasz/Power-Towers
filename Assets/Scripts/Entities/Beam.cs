﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Beam : Entity
{
   
    // Beams are Entities.
    // Special effect, with a time-delayed call for their Effect.
    // Also time until they start fading, and fade time.
    // They have a start and end point.
    // both optionally entities instead.
    // Also they have a duration,
    
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
    
    public void Init(ProjectilePrototype iPrototype, Unit iOriginUnit, Entity iTargetEntity)
    {
        ApplyPrototype(iPrototype);
        OriginUnit = iOriginUnit;
        TargetEntity = iTargetEntity;
    }
    public void Init(ProjectilePrototype iPrototype, Unit iOriginUnit, Vector3 iTargetPoint)
    {
        ApplyPrototype(iPrototype);
        OriginUnit = iOriginUnit;
        TargetPoint = iTargetPoint;
        
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
            TargetPoint = TargetEntity.gameObject.transform.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, TargetPoint, Prototype.Speed * Time.deltaTime);

        Vector3 distance = TargetPoint - transform.position;
        
        if (TargetEntity == null && distance.magnitude < 0.1)
        {
            Prototype.Effect.ApplyPoint(OriginUnit, this, TargetPoint);
        }
        // Move forward based on speed, I think.
        // Turn instantly to face target.
        // 
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
}*/
