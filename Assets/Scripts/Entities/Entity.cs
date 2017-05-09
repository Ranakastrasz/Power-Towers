﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    // Use this for initialization
    protected virtual void Start()
    {
        Redraw();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
        Redraw();
    }

    protected virtual void onRemove()
    {

    }
    
	//Actual object destruction is always delayed until after the current Update loop, but will always be done before rendering.
	// Figure out a fix for this.

	// Validate whether or not it is set to be destroyed?
    public void remove()
    {
        onRemove();
        Destroy(this.gameObject);
    }

    /// <summary>  
    /// Redraw the entity, recoloring it as needed.
    /// </summary>  
    public virtual void Redraw()
    {
        /*float Red = 1;
        float Green = 1;
        float Blue = 1;
        float Alpha = 1;

        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        sprite.material.SetColor("_Color", new Color(Red, Green, Blue, Alpha));*/


    }
}
