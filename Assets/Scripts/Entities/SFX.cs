using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sfx : MonoBehaviour {
    // A Special effect is created and then destroyed after a duration. 
    // Use this for initialization

    public SfxPrototype _prototype { protected set; get; }
    public SpriteRenderer _sprite { protected set; get; }

    public void Init(SfxPrototype iPrototype)
    {
        _sprite = GetComponent<SpriteRenderer>();
        ApplyPrototype(iPrototype);
    }
    public void ApplyPrototype(SfxPrototype iPrototype)
    {
        _prototype = iPrototype;
        _sprite.sprite = iPrototype._sprite;
        gameObject.transform.localScale = new Vector3(iPrototype._size, iPrototype._size, iPrototype._size);
    }

    void Start ()
    {
        
        //Invoke("OnEnd", _prototype._duration);
    }

    private void OnEnd()
    {
        Destroy(this);
    }

    // Update is called once per frame
    void Update () {
		// Unneeded, probably.
	}
}
