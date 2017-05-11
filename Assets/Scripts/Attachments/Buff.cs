using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A buff is attached to a Gameobject, which must be an Entity.
// OnStart occurs when the buff is created.
// OnPeridic occurs every time it's internal timer goes off.
// OnEnd occurs when 
// It self-destructs after a set duration.


public class Buff : MonoBehaviour {

	string Name;
	Entity Source;
	BuffPrototype Prototype;

	public static Component AddBuff(Entity iTarget, BuffPrototype iPrototype, Entity iSource)
	{
		Buff comp = iTarget.gameObject.AddComponent<Buff>();
		comp.Init (iPrototype, iSource);

		return comp;
	}

	public void Init(BuffPrototype iPrototype, Entity iSource)
	{
		Source = iSource;
		ApplyPrototype(iPrototype);

	}

	public void ApplyPrototype(BuffPrototype iPrototype)
	{
		Prototype = iPrototype;
		CancelInvoke ();

		OnStart();
		if (Prototype.Period != 0.0f)
		{
			InvokeRepeating ("OnPeriodic", iPrototype.Period, iPrototype.Period);
		}
		Invoke ("OnEnd", Prototype.Duration);
	}


	private void OnStart()
	{
		Prototype.OnStart.ApplyEntity (Source, Source, gameObject.GetComponent<Entity> ());
	}
	private void OnEnd()
	{
		Prototype.OnEnd.ApplyEntity (Source, Source, gameObject.GetComponent<Entity> ());
		Destroy (this);
	}
	private void OnPeriodic()
	{
		Prototype.OnPeriodic.ApplyEntity (Source, Source, gameObject.GetComponent<Entity> ());
	}

}
