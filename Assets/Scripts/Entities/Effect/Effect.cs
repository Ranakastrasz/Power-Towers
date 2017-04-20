using UnityEngine;

public class Effect
{
    protected bool DestroySource = false;
    

    protected void Impact(Entity iSource)
    {
        // Draw SFX?
        if (DestroySource)
        {
            iSource.remove();
        }
    }

    public virtual void ApplyEntity(Entity iOrigin, Entity iSource, Entity iTarget)
    {
        Impact(iSource);
    }

    public virtual void ApplyPoint(Entity iOrigin, Entity iSource, Vector3 iTarget)
    {
        Impact(iSource);
    }


}
