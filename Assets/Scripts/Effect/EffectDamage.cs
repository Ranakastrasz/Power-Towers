using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDamage : Effect {

    protected int _damage;

    public EffectDamage(int iDamage, bool iDestroy = false)
    {
        _damage = iDamage;
        _destroySource = iDestroy;
    }

    public override void ApplyEntity(Entity iOrigin, Entity iSource, Entity iTarget)
    {

        if (iTarget is Runner)
        {
            Impact(iSource);
            Runner iTargetRunner = iTarget as Runner;
            if (iOrigin is Unit)
            {
                Unit iOriginUnit = iOrigin as Unit;
                iTargetRunner.Damage(iOriginUnit, _damage);
            }
            else
            {
                //SpawnRunner.Damage(SpawnRunner, Damage);
            }
            // Special Effect somehow.
            // CreateSpecialEffect(TargetEntitySFX,iRunner)
        }
        else
        {
            // Only runners can take damage.
        }
    }
    
    


}
