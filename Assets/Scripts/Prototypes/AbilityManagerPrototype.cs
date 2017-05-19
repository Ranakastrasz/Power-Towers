using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManagerPrototype : Prototype
{
    public enum TRIGGER
    { // Abilities occur depending on the situation.
        ON_ATTACK, // On attack, do this instead.
        ON_ATTACK_OVERRIDE, // As ON_ATTACK, but also trip the attack's cooldown.
        CONSTANT // Do at every opportunity.
    }

    public float _cooldown { get; protected set; }
    public int _energyCost { get; protected set; }

    public TRIGGER _trigger { get; protected set; } // Which event causes it to occur.



    public Effect _effect { get; protected set; }

	public string _effectDisplay { get; protected set; } // Purely UI

	public string _name { get; protected set; } // Purely UI

    /*
     * general stuff
     */
	public AbilityManagerPrototype(float iCooldown, int iEnergyCost, TRIGGER iTrigger, Effect iEffect, string iName = "", string iEffectDisplay = "")
    {
        _cooldown = iCooldown;
        _energyCost = iEnergyCost;
        _trigger = iTrigger;
		_effect = iEffect;
		_effectDisplay = iEffectDisplay;
		_name = iName;
    }

}
