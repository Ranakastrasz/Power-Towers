using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePrototype : Prototype
{

    public float _speed { get; protected set; }
	public Effect _effect { get; protected set; }

	public Sprite _sprite { get; protected set;}



	public ProjectilePrototype(float iSpeed, Effect iEffect, Sprite iSprite)
    {
        _speed = iSpeed;
        _effect = iEffect;
		_sprite = iSprite;
    }

}
