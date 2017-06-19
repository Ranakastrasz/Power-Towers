using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxPrototype : Prototype
{
    
	public Sprite _sprite { get; protected set; }
    public float _duration { get; protected set; }
    public float _size { get; protected set; }




    public SfxPrototype(float iDuration, float iSize, Sprite iSprite)
    {
        _duration = iDuration;
		_sprite = iSprite;
        _size = iSize;
    }

}
