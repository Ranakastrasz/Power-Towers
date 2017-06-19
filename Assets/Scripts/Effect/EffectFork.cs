using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Effect Chain, calls each effect. Not a good implimentation, but should work.
public class EffectFork : Effect {
    
    // change to allow a list of Effects, instead of just 4.
    protected Effect _effect1;
    protected Effect _effect2;
    protected Effect _effect3;
    protected Effect _effect4;
    
	public EffectFork(Effect iEffect1, Effect iEffect2, Effect iEffect3, Effect iEffect4, bool iDestroy = false)
    {
        _effect1 = iEffect1;
        _effect2 = iEffect2;
        _effect3 = iEffect3;
        _effect4 = iEffect4;
        _destroySource = iDestroy;
    }

    public override void ApplyEntity(Entity iOrigin, Entity iSource, Entity iTarget)
    {
        _effect1.ApplyEntity(iOrigin, iSource, iTarget);
        _effect2.ApplyEntity(iOrigin, iSource, iTarget);
        _effect3.ApplyEntity(iOrigin, iSource, iTarget);
        _effect4.ApplyEntity(iOrigin, iSource, iTarget);
    }
    
}
