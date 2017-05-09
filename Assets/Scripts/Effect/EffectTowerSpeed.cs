using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTowerSpeed : Effect {

	protected int Damage;

	public EffectTowerSpeed(int iDamage, bool iDestroy)
	{
		Damage = iDamage;
		DestroySource = iDestroy;
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
				iTargetRunner.Damage(iOriginUnit, Damage);
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
