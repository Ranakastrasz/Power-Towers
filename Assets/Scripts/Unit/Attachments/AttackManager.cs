using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    //Attached to a Tower
    //Ontick, find target

    public GameObject CurrentTarget { get; private set; }


    public AttackManagerPrototype Prototype { get; private set; }

    public float CurrentCooldown { get; private set; } // Attack Prototype.


    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    public void ApplyPrototype(AttackManagerPrototype iPrototype)
    {
        Prototype = iPrototype;

    }

    // FixedUpdate is once per physics tick.
    void FixedUpdate()
    {
        ApplyCooldown();
        SearchTarget();
        if (CanAttack())
        {

            if (CurrentTarget != null)
            {
                DoAttack(CurrentTarget);


                /*float targetDistance = (currentTarget.gameObject.transform.position - transform.position).magnitude;
                if (targetDistance > Range)
                {
                    currentTarget = null;
                }
                else
                {

                }*/
            }
        }
    }

    private bool CanAttack()
    {
        return (Prototype.Cooldown != 0) && (CurrentCooldown == Prototype.Cooldown);
    }

    private void ApplyCooldown()
    {
        CurrentCooldown = Mathf.Min( CurrentCooldown + (Time.fixedDeltaTime), Prototype.Cooldown);
    }

    private void DoAttack(GameObject target)
    {
        // Target the CreepData
        Creep creep = target.GetComponent<Creep>();
        // Apply damage. Later create Projectile and attach payload and all that stuff.

        //creep.Damage(this.gameObject.GetComponent<Tower>(), Prototype.Damage);

        Prototype.Effect.ApplyEntity(gameObject.GetComponent<Tower>(), gameObject.GetComponent<Tower>(), creep);

        if (gameObject.GetComponent<PowerManager>().TrySpendEnergy((gameObject.GetComponent<Tower>().Prototype as TowerPrototype).Price))
        { 
            CurrentCooldown = Prototype.Cooldown * 0.75f;
        }
        else
        {
            CurrentCooldown = 0f;
        }

        // Also force facing, send angle to the Towerobject.
    }

    private void SearchTarget()
    {
        GameObject newTarget = null;
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, Prototype.Range);
        float bestValue = 0;
        foreach (Collider currentCollider in collidersInRange)
        {
            GameObject currentEnemy = currentCollider.gameObject;
            Creep creep = currentEnemy.GetComponent<Creep>();
            if (creep != null)
            {
                if (newTarget == null)
                {
                    newTarget = currentEnemy;
                    bestValue = creep.Life;
                }
                else if (bestValue > creep.Life)
                {
                    newTarget = currentEnemy;
                }
            }
        }
        if (newTarget != null)
        {
            CurrentTarget = newTarget;
        }
    }
    
}
