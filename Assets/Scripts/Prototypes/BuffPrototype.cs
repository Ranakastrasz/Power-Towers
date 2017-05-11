using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Merge methods? Not sure,
	Gonna just replace lowest duration buff.

*/

public class BuffPrototype : Prototype
{
	
	public Effect OnStart { get; protected set; }
	public Effect OnEnd { get; protected set; }
	public Effect OnPeriodic { get; protected set; }
	public float Duration { get; protected set; }
	public float Period { get; protected set; }

	public string Name { get; protected set; }
	public string Tooltip { get; protected set; }

	// SFX
	// String Tooltip?

	/*
     * general stuff
     */
	public BuffPrototype(Effect iOnStart, Effect iOnEnd, Effect iOnPeriodic, float iDuration, float iPeriod, string iName, string iTooltip = "")
	{
		OnStart = iOnStart;
		OnEnd = iOnEnd;
		OnPeriodic = iOnPeriodic;
		Duration = iDuration;
		Period = iPeriod;
		Name = iName;
		Tooltip = iTooltip;
	}

}
