using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBuff : Effect {

    // Make a textmacro/easy constructor to chain EffectBuff with EffectMutableStat.

	protected BuffPrototype _buffPrototype;
	protected TARGET _target;

	public EffectBuff(BuffPrototype iBuff, TARGET iTarget, bool iDestroy = false)
	{
		_buffPrototype = iBuff;
		_target = iTarget;
		_destroySource = iDestroy;
	}

	public override void ApplyEntity(Entity iOrigin, Entity iSource, Entity iTarget)
	{
		if (_target == TARGET.RUNNER && iTarget is Runner)
		{
			Impact (iSource);
			if (iOrigin is Unit)
			{
				Unit iOriginUnit = iOrigin as Unit;
				Buff.AddBuff (iTarget, _buffPrototype, iOriginUnit);
			}
			else
			{
				Buff.AddBuff (iTarget, _buffPrototype, iTarget);
			}
		}
		else if (_target == TARGET.TOWER && iTarget is Tower)
		{
			Impact (iSource);
			if (iOrigin is Unit)
			{
				Unit iOriginUnit = iOrigin as Unit;
				Buff.AddBuff (iTarget, _buffPrototype, iOriginUnit);
			}
			else
			{
				Buff.AddBuff (iTarget, _buffPrototype, iTarget);
			}
		}
		else if (_target == TARGET.SELF)
		{
			Impact (iSource);
			Buff.AddBuff (iOrigin, _buffPrototype, iOrigin);
		}
		else
		{
			// Throw Error
		}
	}




}
