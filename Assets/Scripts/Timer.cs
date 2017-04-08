using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        InvokeRepeating("Tick", 1.0f, 1.0f);
    }

    private void Tick()
    {
        PowerManager.GlobalTick();

    }

}
