using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Effect Chain, calls effect on all valid entities in range.
public class EffectSplash : Effect {
    
	protected TARGET _target;
    protected Effect _effect;
    protected float _range;
    
	public EffectSplash(Effect iEffect, float iRange, TARGET iTarget, bool iDestroy = false)
    {
        _effect = iEffect;
        _range = iRange;
        _target = iTarget;
        _destroySource = iDestroy;
    }

    public override void ApplyEntity(Entity iOrigin, Entity iSource, Entity iTarget)
    {
        ApplyPoint(iOrigin, iSource, iTarget.transform.position);
    }
    public override void ApplyPoint(Entity iOrigin, Entity iSource, Vector3 iTarget)
    {
        Impact(iSource);
        Collider[] collidersInRange = Physics.OverlapSphere(iTarget, _range);
        foreach (Collider currentCollider in collidersInRange)
        {
            // For every collider in range

            Entity currentTarget = currentCollider.gameObject.GetComponent<Entity>();
            if (currentTarget)
            { // For all entities
                // Check if it matchs the target Filter
                if (validateTarget(_target, currentTarget))
                {
                    // And if so, apply the effect.
                    _effect.ApplyEntity(iOrigin, iSource, currentTarget);
                }
            }
        }
    }
    
}
