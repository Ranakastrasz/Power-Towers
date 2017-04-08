﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPrototype : Prototype
{

    public float Range { get; protected set; }
    public float Cooldown { get; protected set; }

    public int Damage { get; protected set; } // Will be part of Payload via Projectile later.


    public AttackPrototype(float iRange, float iCooldown, int iDamage)
    {
        Range = iRange;
        Cooldown = iCooldown;
        Damage = iDamage;

    }

}
