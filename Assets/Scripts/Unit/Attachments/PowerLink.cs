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

    public int[] PowerThreshholds = {           0,            1,        25,          125,          625};
    public Color[] BeamColor      = { Color.black, Color.green, Color.blue, Color.yellow, Color.white };

    private int lastTransfer;

    // Use this for initialization
    protected override void Start ()
    {
        lastTransfer = 0;
    }

    public void Init(PowerManager iSource, PowerManager iTarget)
    {
        Source = iSource;
        Target = iTarget;

        float x = iSource.transform.position.x;
        float y = iSource.transform.position.y;

        float dx = iTarget.transform.position.x - x;
        float dy = iTarget.transform.position.x - y;

        float length = Mathf.Sqrt((dx * dx) + (dy * dy));
        float angle = Mathf.Atan2(dy, dx);

        transform.position = iSource.gameObject.transform.position;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

    }

    public void Tick()
    {
        //int EnergyToTransfer = Source.Prototype.TransferRate;
        
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

    }
}
