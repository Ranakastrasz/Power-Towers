using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A buff is attached to a Gameobject, which must be an Entity.
// OnStart occurs when the buff is created.
// OnPeridic occurs every time it's internal timer goes off.
// OnEnd occurs when 
// It self-destructs after a set duration.


public class Buff : MonoBehaviour {

	string _name;
	Entity _source;
	BuffPrototype _prototype;

	public static Component AddBuff(Entity iTarget, BuffPrototype iPrototype, Entity iSource)
	{
		Buff comp = iTarget.gameObject.AddComponent<Buff>();
		comp.Init (iPrototype, iSource);

		return comp;
	}

	public void Init(BuffPrototype iPrototype, Entity iSource)
	{
		_source = iSource;
		ApplyPrototype(iPrototype);

	}

	public void ApplyPrototype(BuffPrototype iPrototype)
	{
		_prototype = iPrototype;
		CancelInvoke ();

		OnStart();
		if (_prototype._Period != 0.0f)
		{
			InvokeRepeating ("OnPeriodic", iPrototype._Period, iPrototype._Period);
		}
		Invoke ("OnEnd", _prototype._duration);
	}


	private void OnStart()
	{
		_prototype._onStart.ApplyEntity (_source, _source, gameObject.GetComponent<Entity> ());
	}
	private void OnEnd()
	{
		_prototype._onEnd.ApplyEntity (_source, _source, gameObject.GetComponent<Entity> ());
		Destroy (this);
	}
	private void OnPeriodic()
	{
		_prototype._onPeriodic.ApplyEntity (_source, _source, gameObject.GetComponent<Entity> ());
	}

}
