using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectProjectile : Effect {

    protected ProjectilePrototype _projectilePrototype;

    public EffectProjectile(ProjectilePrototype iProjectilePrototype, bool iDestroy = false)
    {
        _projectilePrototype = iProjectilePrototype;
        _destroySource = iDestroy;
    }
   
    public override void ApplyEntity(Entity iOrigin, Entity iSource, Entity iTarget)
    {
        base.Impact(iSource);
        if (iOrigin is Unit)
        {
            EntityManager.CreateProjectile(iOrigin as Unit, iSource.gameObject.transform.position, iTarget, _projectilePrototype);
        }
        else
        {
            EntityManager.CreateProjectile(null, iSource.gameObject.transform.position, iTarget, _projectilePrototype);
        }
    }

    public override void ApplyPoint(Entity iOrigin, Entity iSource, Vector3 iTargetPoint)
    {
        base.Impact(iSource);
        
        //EntityManager.CreateProjectile(null, iSource.gameObject.transform.position, iTarget, _projectilePrototype);

    }

}
