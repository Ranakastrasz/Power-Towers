using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    protected IDictionary<string,MutableStat> _statTable;


    public virtual void AddStatMod(string iStatKey, string iSourceKey,  float iValue)
    {
        _statTable[iStatKey].ModifyMult(iSourceKey, iValue);
        RecalculateStats();
    }

    public virtual void RemoveStatMod(string iStatkey, string iSourceKey)
    {
        _statTable[iStatkey].UnmodifyMult(iSourceKey);
        RecalculateStats();
    }

    public virtual float GetStat(string iStatKey, bool baseValue = false)
    {
        if (baseValue)
        {
            return _statTable[iStatKey].baseValue;
        }
        else
        {
            return _statTable[iStatKey].modifiedValue;
        }
    }

    /// <summary>
    /// This function is called when mutable stats change
    /// </summary>
    protected virtual void RecalculateStats()
    {

    }

    // Use this for initialization
    protected virtual void Start()
    {
        _statTable = new Dictionary<string, MutableStat>();
        Redraw();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
        Redraw();
    }

    protected virtual void onRemove()
    {

    }
    
	//Actual object destruction is always delayed until after the current Update loop, but will always be done before rendering.
	// Figure out a fix for this.

	// Validate whether or not it is set to be destroyed?
    public void remove()
    {
        onRemove();
        Destroy(this.gameObject);
    }

    /// <summary>  
    /// Redraw the entity, recoloring it as needed.
    /// </summary>  
    public virtual void Redraw()
    {
        /*float Red = 1;
        float Green = 1;
        float Blue = 1;
        float Alpha = 1;

        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        sprite.material.SetColor("_Color", new Color(Red, Green, Blue, Alpha));*/


    }
}
