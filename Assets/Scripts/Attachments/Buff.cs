using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A buff is attached to a Gameobject, which must be an Entity.
// OnStart occurs when the buff is created.
// OnPeridic occurs every time it's internal timer goes off.
// OnEnd occurs when 
// It self-destructs after a set duration.


public class Buff : MonoBehaviour {

	Entity source;
	BuffPrototype prototype;

	public static Component AddBuff(Entity iTarget, BuffPrototype iPrototype, Entity iSource)
	{
		Buff comp = iTarget.gameObject.AddComponent<Buff>();
		comp.Init (iPrototype, iSource);

		return comp;
	}

	public void Init(BuffPrototype iPrototype, Entity iSource)
	{
		source = iSource;
		ApplyPrototype(iPrototype);

	}

	public void ApplyPrototype(BuffPrototype iPrototype)
	{
		prototype = iPrototype;
		CancelInvoke ();

		OnStart();
		if (prototype.Period != 0.0f)
		{
			InvokeRepeating ("OnPeriodic", iPrototype.Period, iPrototype.Period);
		}
		Invoke ("OnEnd", prototype.Duration);
	}


	private void OnStart()
	{
		prototype.OnStart.ApplyEntity (source, source, gameObject.GetComponent<Entity> ());
	}
	private void OnEnd()
	{
		prototype.OnEnd.ApplyEntity (source, source, gameObject.GetComponent<Entity> ());
	}
	private void OnPeriodic()
	{
		prototype.OnPeriodic.ApplyEntity (source, source, gameObject.GetComponent<Entity> ());
	}

}
