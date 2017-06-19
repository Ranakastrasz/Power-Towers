using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMutableStat : Effect
{
    // Effects the Target, hardcoded. 
    protected string _statKey;
    protected string _sourceKey;
    protected bool _add;
	protected float _mod;

	public EffectMutableStat(string iStatKey, string iSourceKey, float iMod, bool iAdd, bool iDestroy = false)
	{
        _statKey = iStatKey;
        _sourceKey = iSourceKey;
        _add = iAdd;
        _mod = iMod;
        _destroySource = iDestroy;
    }

	public EffectMutableStat(string iStatKey, string iSourceKey, bool iAdd, bool iDestroy = false)
    {
        _statKey = iStatKey;
        _sourceKey = iSourceKey;
        _add = iAdd;
        _mod = 0f;
        _destroySource = iDestroy;
    }

    public override void ApplyEntity(Entity iOrigin, Entity iSource, Entity iTarget)
{
		Impact (iTarget);
		if (_add)
		{
			iTarget.AddStatMod (_statKey, _sourceKey, _mod);
		}
		else
		{
			iTarget.RemoveStatMod (_statKey, _sourceKey);
		}
		
	}
}
