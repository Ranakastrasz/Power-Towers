using UnityEngine;

public class Effect
{
	// Effects are an awful lot like Prototypes.
	// They are just a collection of data on how to do something, rather than a concrete object.

    protected bool _destroySource = false;
    
	public enum TARGET
	{
		SELF,
		RUNNER,
		TOWER
	}

    protected void Impact(Entity iSource)
    {
        // Draw SFX?
        if (_destroySource)
        {
            iSource.remove();
        }
    }

    public virtual void ApplyEntity(Entity iOrigin, Entity iSource, Entity iTarget)
    {
        Impact(iSource);
    }

    public virtual void ApplyPoint(Entity iOrigin, Entity iSource, Vector3 iTargetPoint)
    {
        Impact(iSource);
    }


}
