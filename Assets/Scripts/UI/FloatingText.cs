using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : Entity {

    // has timer till die.
    // Has fade-rate, proportional to time elapsed, fades to nothing at end of timer, then dies.
    // Has text to set, color, font, maybe.

    private float _speed;
    private Color _color;
    private float _duration;
    private float _durationLeft;

	public void Init(string iText, float iDuration, Color iColor)
	{
        //Speed = iSpeed;
        _color = iColor;
        _duration = iDuration;
        _durationLeft = iDuration;

        GetComponent<TextMesh>().text = iText;
        GetComponent<MeshRenderer>().sortingLayerName = "UI";
        GetComponent<MeshRenderer>().sortingOrder = 3;
        
        transform.Translate(new Vector3(Random.Range(-0.5f,0.5f),Random.Range(0f,0.5f),0f));


        _speed = 1.0f;

		Invoke ("OnEnd", iDuration);
        Redraw();
	}
    
    void OnEnd()
    {
        Destroy (this.gameObject);
    }

	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();

        _durationLeft -= Time.deltaTime;
        // See if you can automate this instead. Rigidbody -> Velocity?
        transform.Translate(new Vector3(0, _speed * Time.deltaTime, 0));

	}

    public override void Redraw()
    {
        GetComponent<TextMesh>().color = new Color(_color.r,_color.g,_color.b,_durationLeft/_duration);

    }
}
