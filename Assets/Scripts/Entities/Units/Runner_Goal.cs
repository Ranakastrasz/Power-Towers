﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner_Goal : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entered");

        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        //if (other.gameObject.CompareTag("PickUp"))
        //{
        //this.GetComponent<Collider2D>().gameObject.SetActive(false);
		if (other.gameObject.tag == "PointCollider") {
			Runner runner = other.transform.parent.GetComponent<Runner> ();
			if (runner != null)
			{
				Destroy (runner.gameObject);
				Player.Active.DrainLife (1);
			}
        }
        //Destroy(this);
        //}
    }
    void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exited");
    }
    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Stay");
    }
}
