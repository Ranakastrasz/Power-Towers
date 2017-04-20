using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDamage : Effect {

    protected int Damage;

    public EffectDamage(int iDamage, bool iDestroy)
    {
        Damage = iDamage;
        DestroySource = iDestroy;
    }

    public override void ApplyEntity(Entity iOrigin, Entity iSource, Entity iTarget)
    {

        if (iTarget is Creep)
        {
            Impact(iSource);
            Creep iTargetCreep = iTarget as Creep;
            if (iOrigin is Unit)
            {
                Unit iOriginUnit = iOrigin as Unit;
                iTargetCreep.Damage(iOriginUnit, Damage);
            }
            else
            {
                //iTargetCreep.Damage(iTargetCreep, Damage);
            }
            // Special Effect somehow.
            // CreateSpecialEffect(TargetEntitySFX,iCreep)
        }
        else
        {
            // Only creeps can take damage.
        }
    }
    
    


}
