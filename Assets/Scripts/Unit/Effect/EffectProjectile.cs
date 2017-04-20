using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectProjectile : Effect {

    protected ProjectilePrototype ProjectilePrototype;

    public EffectProjectile(ProjectilePrototype iProjectilePrototype, bool iDestroy)
    {
        ProjectilePrototype = iProjectilePrototype;
        DestroySource = iDestroy;
    }
   
    public override void ApplyEntity(Entity iOrigin, Entity iSource, Entity iTarget)
    {
        base.Impact(iSource);
        if (iOrigin is Unit)
        {
            EntityManager.CreateProjectile(iOrigin as Unit, iSource.gameObject.transform.position, iTarget, ProjectilePrototype);
        }
        else
        {
            EntityManager.CreateProjectile(null, iSource.gameObject.transform.position, iTarget, ProjectilePrototype);
        }
    }

    public override void ApplyPoint(Entity iOrigin, Entity iSource, Vector3 iTarget)
    {
        base.Impact(iSource);

        // EntityManager.CreateProjectile(ProjectilePrototype, iOrigin, iSource, null)

    }

}
