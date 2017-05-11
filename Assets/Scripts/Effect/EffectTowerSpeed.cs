using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTowerSpeed : Effect
{
    // HARDCODED CURRENTLY TO EFFECT THE SOURCE TOWER
    
    // Later a general stat modifier.
    // Also let you chose the target entity.
	protected string Key;
	protected bool Add;
	protected float Mod;

	public EffectTowerSpeed(string iKey, float iMod, bool iAdd)
	{
        Key = iKey;
        Add = iAdd;
        Mod = iMod;
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
                if (Add)
                {
                    iTargetTower.AttackManager.AttackSpeed.ModifyMult(Key, Mod, Key);
                }
                else
                {
                    iTargetTower.AttackManager.AttackSpeed.UnmodifyMult(Key);
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
