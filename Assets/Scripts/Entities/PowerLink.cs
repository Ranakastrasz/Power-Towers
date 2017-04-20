using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This entity handles a power transfer connection between two Power Managers, and thus towers.
 */

public class PowerLink : Entity {

    public const float RANGE_SHORT = 3.7f;
    public const float RANGE_LONG = 11.0f;

    public const int MAX_LINKS = 8;

    public PowerManager Source { get; protected set; }
    public PowerManager Target { get; protected set; }


    public readonly static int[] PowerThreshholds = {           0,            1,        25,          125,          625};
    public readonly static Color[] BeamColor      = { new Color(1.0f,0.5f,0.0f), Color.green, Color.blue, Color.yellow, Color.white };

    /// <summary>
    ///  How much power was transfered last tick, for drawing the beam.
    /// </summary>
    private int lastTransfer;
    
    public enum LINK_RANGE
        {
            /// <summary> Withing RANGE_SHORT of each other</summary>
            SHORT,
            /// <summary> Withing RANGE_LONG of each other</summary>
            LONG,
            /// <summary> too far away to connect.</summary>
            INVALID
        };

    public LINK_RANGE LinkRange { get; protected set; }

    // Use this for initialization
    protected override void Start ()
    {
    }


    public void Init(PowerManager iSource, PowerManager iTarget)
    {
        Source = iSource;
        Target = iTarget;

        lastTransfer = 0;

        Vector3 targetPosition = iTarget.gameObject.transform.position;
        Vector3 sourcePosition = iSource.gameObject.transform.position;

        /*float x = iTarget.gameObject.transform.position.x;
        float y = iTarget.gameObject.transform.position.y;

        float dx = iSource.gameObject.transform.position.x - x;
        float dy = iSource.gameObject.transform.position.y - y;

        float length = Mathf.Sqrt((dx * dx) + (dy * dy));
        float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        
        transform.position = iSource.gameObject.transform.position;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        transform.localScale = new Vector3(length, 1.0f, 1.0f);*/
        
        Vector3 relativePos = targetPosition - sourcePosition;

        transform.position = sourcePosition;

        float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.localScale = new Vector3(relativePos.magnitude, 1.0f, 1.0f);

        LinkRange = getLinkRange(iSource, iTarget);

        /*Vector3 objectScale = itemBeingPlaced.localScale;
        float distance = Vector3.Distance(hit.point, itemBeingPlaced.position);
        Vector3 newScale = new Vector3(objectScale.x, objectScale.y, distance);
        itemBeingPlaced.localScale = newScale;*/
    }
    

    public static LINK_RANGE getLinkRange(PowerManager iSource, PowerManager iTarget)
    {
        Vector3 targetPosition = iTarget.gameObject.transform.position;
        Vector3 sourcePosition = iSource.gameObject.transform.position;

        Vector3 relativePos = targetPosition - sourcePosition;

        /*transform.position = sourcePosition;

        float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/

        float distance = relativePos.magnitude;
        LINK_RANGE linkType;
        if (distance <= RANGE_SHORT)
        {
            linkType = LINK_RANGE.SHORT;
        }
        else if (distance <= RANGE_LONG)
        {
            linkType = LINK_RANGE.LONG;
        }
        else
        {
            linkType = LINK_RANGE.INVALID;
        }

        return linkType;
    }

    public static bool isInRange(PowerManager iSource, PowerManager iTarget, bool longLink)
    {
        LINK_RANGE linkRange = getLinkRange(iSource, iTarget);
        
        if (linkRange == LINK_RANGE.SHORT)
        {
            // All links can link at short range.
            return true;
        }

        if (linkRange == LINK_RANGE.LONG && longLink)
        {
            // if it is a long range link, it has to be a long link, or it won't work.
            return true;
        }

        return false;
        
    }


    protected override void onRemove()
    {
        Source.PowerLinksOut.Remove(this);
        Target.PowerLinksIn.Remove(this);
    }


    public override void Redraw()
    {
        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        int index = 0;
        int z;
        for (z = 0; z < PowerThreshholds.Length; z++)
        {
            if (lastTransfer >= PowerThreshholds[z])
            {
                index = z;
            }
        }
        

        sprite.material.SetColor("_Color", BeamColor[index]);

        float animationSpeed = (float)index * 0.25f;

        Animator anim = this.GetComponent<Animator>();
        
        anim.speed = animationSpeed;
    }

    /// <summary>
    /// Push as much energy to the target as possible through this PowerLink.
    /// </summary>
    /// <param name="iEnergyToTransfer"></param>
    public void PushEnergy(int iEnergyToTransfer)
    {
        int energyToRecieve = Math.Min(Target.Prototype.MaxEnergy - Target.Energy, Target.Prototype.TransferRate - Target.totalReceived);

        int energyToTransfer = Math.Min(iEnergyToTransfer, energyToRecieve);

        // Not sure how to handle this. Would prefer not having to make this part public.
        Source.Energy -= energyToTransfer;
        Target.Energy += energyToTransfer;

        Source.totalSent += energyToTransfer;
        Target.totalReceived += energyToTransfer;
        
        lastTransfer = energyToTransfer;
    }
    

}
