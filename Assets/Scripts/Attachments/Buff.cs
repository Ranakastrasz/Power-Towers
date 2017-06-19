using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A buff is attached to a Gameobject, which must be an Entity.
// OnStart occurs when the buff is created.
// OnPeridic occurs every time it's internal timer goes off.
// OnEnd occurs when 
// It self-destructs after a set duration.


public class Buff : MonoBehaviour {

	private string _name;
    private Entity _source;
    private BuffPrototype _prototype;
    private float timeElapsed;

    public static Component AddBuff(Entity iTarget, BuffPrototype iPrototype, Entity iSource)
    {
        if (iPrototype._unique) // quick and dirty uniqueness-getter thinggy. Ideally I need a comparison to drop weakest version, but that is hard.
        {
            // Find all existing buffs of this type.
            Buff[] buffs = iTarget.GetComponents<Buff>();
            foreach (Buff buff in buffs)
            {
                if (buff._prototype._name.Equals(iPrototype._name))
                {
                    Destroy(buff);
                    //if (iPrototype._duration > component.)
                }
            }
        }
		Buff newBuff = iTarget.gameObject.AddComponent<Buff>();
        newBuff.Init (iPrototype, iSource);

		return newBuff;
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
		if (_prototype._period > 0f)
		{
			InvokeRepeating ("OnPeriodic", iPrototype._period, iPrototype._period);
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

    public void Update()
    {

    }

}
