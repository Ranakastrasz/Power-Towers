using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback : MonoBehaviour {

    public const float DRAIN_PER_DAMAGE = 0.1f;
    //Attached to a Tower
    //Ontick, find target
    
    public Runner _parent { get; private set; }
    



    void Start()
    {
        _parent = gameObject.GetComponent<Runner>();
        _parent._onDamageEvent += new OnDamageEventHandler(OnDamage);

    }

    void OnDamage(Unit iSource, int iDamage)
    {
        if (iSource is Tower)
        {
            Tower tower = iSource as Tower;
            tower._powerManager.DrainEnergy((int)(iDamage * DRAIN_PER_DAMAGE));
        }
    }

    void OnDestroy()
    {
        _parent._onDamageEvent -= this.OnDamage; // not sure if this will work.
    }


}
