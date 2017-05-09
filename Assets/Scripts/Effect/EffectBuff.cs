using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBuff : Effect {

	public enum TARGET
	{
		SELF,
		RUNNER,
		TOWER
	}

	protected BuffPrototype buff;
	protected TARGET target;

	public EffectBuff(BuffPrototype iBuff, TARGET iTarget, bool iDestroy)
	{
		buff = iBuff;
		target = iTarget;
		DestroySource = iDestroy;
	}

	public override void ApplyEntity(Entity iOrigin, Entity iSource, Entity iTarget)
	{

		if (target == TARGET.RUNNER && iTarget is Runner)
		{
			Impact (iSource);
			Runner iTargetRunner = iTarget as Runner;
			if (iOrigin is Unit)
			{
				Unit iOriginUnit = iOrigin as Unit;
				Buff.AddBuff (iTargetRunner, buff, iOriginUnit);
			}
			else
			{
				Buff.AddBuff (iTargetRunner, buff, iTargetRunner);
			}
		}
		else if (target == TARGET.TOWER && iTarget is Tower)
		{
			Impact (iSource);
			Tower iTargetTower = iTarget as Tower;
			if (iOrigin is Unit)
			{
				Unit iOriginUnit = iOrigin as Unit;
				Buff.AddBuff (iTargetTower, buff, iOriginUnit);
			}
			else
			{
				Buff.AddBuff (iTargetTower, buff, iTargetTower);
			}
		}
		else
		{
			// Only runners can take damage.
		}
	}




}
