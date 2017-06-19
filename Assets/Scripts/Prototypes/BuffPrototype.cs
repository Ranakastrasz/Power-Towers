using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Merge methods? Not sure,
	Gonna just replace lowest duration buff.

*/

public class BuffPrototype : Prototype
{
	
	public Effect _onStart { get; protected set; }
	public Effect _onEnd { get; protected set; }
	public Effect _onPeriodic { get; protected set; }
	public float _duration { get; protected set; }
	public float _period { get; protected set; }

	public string _name { get; protected set; }
	public string _tooltip { get; protected set; }

    public bool _unique { get; protected set; }

	// SFX
	// String Tooltip?

	/*
     * general stuff
     */
	public BuffPrototype(Effect iOnStart, Effect iOnEnd, Effect iOnPeriodic, float iDuration, float iPeriod, bool iUnique, string iName, string iTooltip = "")
	{
		_onStart = iOnStart;
		_onEnd = iOnEnd;
		_onPeriodic = iOnPeriodic;
		_duration = iDuration;
		_period = iPeriod;
        _unique = iUnique;
		_name = iName;
		_tooltip = iTooltip;
	}

}
