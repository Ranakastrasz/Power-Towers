using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTowerSpeed : Effect
{
    // HARDCODED CURRENTLY TO EFFECT THE SOURCE TOWER
    
    // Later a general stat modifier.
    // Also let you chose the target entity.
	protected string _key;
	protected bool _add;
	protected float _mod;

	public EffectTowerSpeed(string iKey, float iMod, bool iAdd)
	{
        _key = iKey;
        _add = iAdd;
        _mod = iMod;
	}

	public override void ApplyEntity(Entity iOrigin, Entity iSource, Entity iTarget)
	{

		if (iTarget is Tower)
		{
			Impact(iTarget);
			Tower iTargetTower = iTarget as Tower;
            if (iTarget is Unit)
            {
                //Unit iOriginUnit = iTarget as Unit;
                if (_add)
                {
                    iTargetTower._attackManager._attackSpeed.ModifyMult(_key, _mod, _key);
                }
                else
                {
                    iTargetTower._attackManager._attackSpeed.UnmodifyMult(_key);
                }
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
