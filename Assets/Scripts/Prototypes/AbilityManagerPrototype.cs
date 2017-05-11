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

    public float Cooldown { get; protected set; }
    public int EnergyCost { get; protected set; }

    public TRIGGER Trigger { get; protected set; } // Which event causes it to occur.



    public Effect Effect { get; protected set; }

	public string EffectDisplay { get; protected set; } // Purely UI

	public string Name { get; protected set; } // Purely UI

    /*
     * general stuff
     */
	public AbilityManagerPrototype(float iCooldown, int iEnergyCost, TRIGGER iTrigger, Effect iEffect, string iName = "", string iEffectDisplay = "")
    {
        Cooldown = iCooldown;
        EnergyCost = iEnergyCost;
        Trigger = iTrigger;
		Effect = iEffect;
		EffectDisplay = iEffectDisplay;
		Name = iName;
    }

}
