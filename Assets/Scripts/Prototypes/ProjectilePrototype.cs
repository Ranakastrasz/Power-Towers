using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePrototype : Prototype
{

    public float Speed { get; protected set; }
	public Effect Effect { get; protected set; }

	public Sprite Sprite { get; protected set;}



	public ProjectilePrototype(float iSpeed, Effect iEffect, Sprite iSprite)
    {
        Speed = iSpeed;
        Effect = iEffect;
		Sprite = iSprite;
    }

}
