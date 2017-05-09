using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManagerPrototype : Prototype
{

    public float Range { get; protected set; }
    public float Cooldown { get; protected set; }
    
    public int DamageDisplay { get; protected set; } // Purely UI

    public Effect Effect { get; protected set; }
        
    /*
     * general stuff
     */
	public AttackManagerPrototype(float iRange, float iCooldown, int iDamageDisplay, Effect iEffect)
    {
        Range = iRange;
        Cooldown = iCooldown;
		DamageDisplay = iDamageDisplay;
        Effect = iEffect;
    }

}
