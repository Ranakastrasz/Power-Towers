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
			if (iOrigin is Unit)
			{
				Unit iOriginUnit = iOrigin as Unit;
				Buff.AddBuff (iTarget, buff, iOriginUnit);
			}
			else
			{
				Buff.AddBuff (iTarget, buff, iTarget);
			}
		}
		else if (target == TARGET.TOWER && iTarget is Tower)
		{
			Impact (iSource);
			if (iOrigin is Unit)
			{
				Unit iOriginUnit = iOrigin as Unit;
				Buff.AddBuff (iTarget, buff, iOriginUnit);
			}
			else
			{
				Buff.AddBuff (iTarget, buff, iTarget);
			}
		}
		else if (target == TARGET.SELF)
		{
			Impact (iSource);
			Buff.AddBuff (iOrigin, buff, iOrigin);
		}
		else
		{
			// Throw Error
		}
	}




}
