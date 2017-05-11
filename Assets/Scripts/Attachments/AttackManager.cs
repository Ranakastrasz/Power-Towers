using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    //Attached to a Tower
    //Ontick, find target

    public GameObject CurrentTarget { get; private set; }


	public AttackManagerPrototype Prototype { get; private set; }
	public AbilityManagerPrototype AbilityPrototype { get; private set; }


	public float CooldownRemaining { get; private set; }

	public float AbilityCooldownRemaining { get; private set; }

	public MutableStat AttackSpeed { get; private set; }



	public void ApplyPrototype(AttackManagerPrototype iPrototype, AbilityManagerPrototype iAbilityPrototype)
	{
		Prototype = iPrototype;
		AbilityPrototype = iAbilityPrototype;
		if (AttackSpeed == null)
		{
			AttackSpeed = new MutableStat();
		}
		AttackSpeed.baseValue = 1;
		CooldownRemaining = Prototype.Cooldown;
		AbilityCooldownRemaining = AbilityPrototype.Cooldown;

    }

    // FixedUpdate is once per physics tick.
    void FixedUpdate()
    {
        ApplyCooldown();
		CheckTarget ();
        SearchTarget();
        if (CanAttack())
        {

            if (CurrentTarget != null)
            {
                TryAttack(CurrentTarget);

            }
        }
		if (AbilityPrototype.Trigger == AbilityManagerPrototype.TRIGGER.CONSTANT && CanCast())
		{
			if (CurrentTarget != null)
			{
				DoCast(CurrentTarget);

			}
		}
    }

	private void CheckTarget()
	{      
		if (CurrentTarget != null)
		{
			Vector3 closestPoint = CurrentTarget.GetComponent<Collider> ().ClosestPointOnBounds (transform.position);
			float distance = Vector3.Distance (closestPoint, transform.position);
			if (distance > Prototype.Range)
			{
				CurrentTarget = null;
			}
		}
	}

	private bool CanCast()
	{
		return (AbilityPrototype.Cooldown != 0.0f) &&(AbilityCooldownRemaining <= 0) && (gameObject.GetComponent<PowerManager>().CanSpendEnergy(AbilityPrototype.EnergyCost));
	}
    private bool CanAttack()
    {
        return (CooldownRemaining <= 0);
    }

    private void ApplyCooldown()
	{
		CooldownRemaining = Mathf.Max( CooldownRemaining - ((Time.fixedDeltaTime)*AttackSpeed.modifiedValue), 0);
		// Ability cooldowns are uneffected.
		AbilityCooldownRemaining = Mathf.Max( AbilityCooldownRemaining - ((Time.fixedDeltaTime)), 0);
    }

	private void TryAttack(GameObject target)
	{

		if (CanCast ())
		{
			DoCast (target);
		}
		else
		{
			DoAttack(target);
		}

	}
	private void DoAttack(GameObject target)
	{
        // Target the RunnerData
        Runner runner = target.GetComponent<Runner>();

		Prototype.Effect.ApplyEntity(gameObject.GetComponent<Tower>(), gameObject.GetComponent<Tower>(), runner);
		CooldownRemaining = Prototype.Cooldown;

    }

	private void DoCast(GameObject target)
	{
		Runner runner = target.GetComponent<Runner>();
		if (gameObject.GetComponent<PowerManager> ().TrySpendEnergy (AbilityPrototype.EnergyCost))
		{
			AbilityPrototype.Effect.ApplyEntity (gameObject.GetComponent<Tower> (), gameObject.GetComponent<Tower> (), runner);
			AbilityCooldownRemaining = AbilityPrototype.Cooldown;
			if (AbilityPrototype.Trigger == AbilityManagerPrototype.TRIGGER.ON_ATTACK_OVERRIDE)
			{
				CooldownRemaining = Mathf.Min(AbilityPrototype.Cooldown,Prototype.Cooldown);
			}
			else if (AbilityPrototype.Trigger == AbilityManagerPrototype.TRIGGER.ON_ATTACK)
			{
				// Simulate casting time somehow? At least delay the next attack a bit.
				CooldownRemaining = Mathf.Max(CooldownRemaining,0.1f);
			}
		}

	}
    private void SearchTarget()
    {
        GameObject newTarget = null;
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, Prototype.Range);
        float bestValue = 0;
        foreach (Collider currentCollider in collidersInRange)
        {
            GameObject currentEnemy = currentCollider.gameObject;
            Runner runner = currentEnemy.GetComponent<Runner>();
            if (runner != null)
            {
                if (newTarget == null)
                {
                    newTarget = currentEnemy;
                    bestValue = runner.Life;
                }
                else if (bestValue > runner.Life)
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
