using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creep_Goal : Unit
{

    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entered");

        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        //if (other.gameObject.CompareTag("PickUp"))
        //{
        //this.GetComponent<Collider2D>().gameObject.SetActive(false);
        Destroy(other.gameObject);
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
