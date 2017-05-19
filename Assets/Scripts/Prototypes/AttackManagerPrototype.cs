using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManagerPrototype : Prototype
{

    public float _range { get; protected set; }
    public float _cooldown { get; protected set; }
    
    public int _damageDisplay { get; protected set; } // Purely UI

    public Effect _effect { get; protected set; }
        
    /*
     * general stuff
     */
	public AttackManagerPrototype(float iRange, float iCooldown, int iDamageDisplay, Effect iEffect)
    {
        _range = iRange;
        _cooldown = iCooldown;
		_damageDisplay = iDamageDisplay;
        _effect = iEffect;
    }

}
