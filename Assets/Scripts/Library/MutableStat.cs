using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *	The Mutable Stat is a replacemenet for a semi-constant variable. 
 *  It has a base value, and a set of modifiers applied to it.
 *  The Modifiers have string keys, which define their source, as well as type (linear/Percentage)
 *  and magnitude.
 *  
 *  For Instant, RunnerSpeed would have a base value of 3, and would get a 1.3x modifier from Speed rounds, a 0.5x from frost debuff
 *  And 0.0x from Root debuff.
 * 
 *  If speed is applied, 3 * (1.3) = 3.9 speed.
 *  If you also apply Frost, 3 * (1.3) * (0.5) = 3.9 * 0.5 = 1.95 speed.
 *  
 *  Linear modifiacations will also be possible, but I can't think where to use them yet.
 *  Say, damage bonuses, even though the game wont use them.
 *  
 *  100 Damage, +30, +40, +20;
 * 
 * Also, hybrid, +30, x130%, would apply linear first. so 130 * 1.3 or 169 damage.
 * 
 */

/*
	var dict = new Dictionary<string, string>();
	dict["key_name"] = "value1";
*/

// Stat mod is a single mod, used for each modifier.


public class StatMod
{
	public string description{ get; private set; }
	public string source{ get; private set; }
	public float value{ get; private set; }

	public StatMod( string iSource, float iValue,string iDescription = "")
	{
		description = iDescription;
		source = iSource;
		value = iValue;
	}

}


public class MutableStat
{
	public MutableStat()
	{
		multMods = new Dictionary<string, StatMod>();
		baseValue = 0f;
	}

    private void CalculateModifiedValue()
    {
        modifiedValue = baseValue;
        foreach (KeyValuePair<string, StatMod> entry in multMods)
        {
            modifiedValue *= entry.Value.value;
        }
    }

    public void ModifyMult(string iKey, float iValue, string iDescription = "")
    {
        if (!multMods.ContainsKey(iKey))
        {
            StatMod mod = new StatMod(iKey, iValue, iDescription);
            multMods.Add(iKey, mod);
        }
        else
        {
            // throw warning
        }

        CalculateModifiedValue();
    }

    public void UnmodifyMult(string iKey)
    {
        if (multMods.ContainsKey(iKey))
        {
            multMods.Remove(iKey);
        }
        else
        {
            // throw warning
        }
        
        CalculateModifiedValue();
    }


	private float _basevalue;
	public float baseValue
	{
		get
		{
			return _basevalue;
		}
		set
		{
			_basevalue = value;
			CalculateModifiedValue ();
		}
	}

    public float modifiedValue { get; private set; }

	//private Dictionary<string, StatMod> flatMods;
	private Dictionary<string, StatMod> multMods;



}
