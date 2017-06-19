using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    //Attached to a Tower
    //Ontick, find target

    public GameObject _currentTarget { get; private set; }

    public Tower _parent { get; private set; }

	public AttackManagerPrototype _prototype { get; private set; }
	public AbilityManagerPrototype _abilityPrototype { get; private set; }


	public float _cooldownRemaining { get; private set; }

	public float _abilityCooldownRemaining { get; private set; }
    



    void Start()
    {
        _parent = gameObject.GetComponent<Tower>();

    }

    public void ApplyPrototype(AttackManagerPrototype iPrototype, AbilityManagerPrototype iAbilityPrototype)
	{
		_prototype = iPrototype;
		_abilityPrototype = iAbilityPrototype;
		_cooldownRemaining = _prototype._cooldown;
		_abilityCooldownRemaining = _abilityPrototype._cooldown;

    }

    // FixedUpdate is once per physics tick.
    void FixedUpdate()
    {
        ApplyCooldown(); 
		CheckTarget ();
        SearchTarget();
        if (CanAttack())
        {

            if (_currentTarget != null)
            {
                TryAttack(_currentTarget);

            }
        }
		if (_abilityPrototype._trigger == AbilityManagerPrototype.TRIGGER.CONSTANT && CanCast())
		{
			if (_currentTarget != null)
			{
				DoCast(_currentTarget);

			}
		}
    }

	private void CheckTarget()
	{      
		if (_currentTarget != null)
		{
			Vector3 closestPoint = _currentTarget.GetComponent<Collider> ().ClosestPointOnBounds (transform.position);
			float distance = Vector3.Distance (closestPoint, transform.position);
			if (distance > _prototype._range)
			{
				_currentTarget = null;
			}
		}
	}

	private bool CanCast()
	{
		return (_abilityPrototype._cooldown != 0.0f) &&(_abilityCooldownRemaining <= 0) && (gameObject.GetComponent<PowerManager>().CanSpendEnergy(_abilityPrototype._energyCost));
	}
    private bool CanAttack()
    {
        return (_cooldownRemaining <= 0);
    }

    private void ApplyCooldown()
	{
		_cooldownRemaining = Mathf.Max( _cooldownRemaining - ((Time.fixedDeltaTime)*_parent.GetStat("attackSpeed")), 0);
		// Ability cooldowns are uneffected.
		_abilityCooldownRemaining = Mathf.Max( _abilityCooldownRemaining - ((Time.fixedDeltaTime)), 0);
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

		_prototype._effect.ApplyEntity(_parent, _parent, runner);
		_cooldownRemaining = _prototype._cooldown;

    }

	private void DoCast(GameObject target)
	{
		Runner runner = target.GetComponent<Runner>();
		if (gameObject.GetComponent<PowerManager> ().TrySpendEnergy (_abilityPrototype._energyCost))
		{
			_abilityPrototype._effect.ApplyEntity (_parent, _parent, runner);
			_abilityCooldownRemaining = _abilityPrototype._cooldown;
			if (_abilityPrototype._trigger == AbilityManagerPrototype.TRIGGER.ON_ATTACK_OVERRIDE)
			{
				_cooldownRemaining = Mathf.Min(_abilityPrototype._cooldown,_prototype._cooldown);
			}
			else if (_abilityPrototype._trigger == AbilityManagerPrototype.TRIGGER.ON_ATTACK)
			{
				// Simulate casting time somehow? At least delay the next attack a bit.
				_cooldownRemaining = Mathf.Max(_cooldownRemaining,0.1f);
			}
		}

	}
    private void SearchTarget()
    {
        GameObject newTarget = null;
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, _prototype._range);
        float bestValue = 0;
        foreach (Collider currentCollider in collidersInRange)
        {
            GameObject currentEnemy = currentCollider.gameObject;
            // Much cheaper check. When it succeeds it is more expensive, slightly, but it ignores every other tower and projectile.
            // Almost literally cuts it from O(N^2) to O(N). Or rather O(N^3) to O(n^2) overall.
            // After all, it went from "For each projectile, check all entities on the map using this expensive function" to "Use this cheap function that is just a lookup.
            // Apperently the other function is more expensive than a generic lookup.
            if (currentEnemy.tag == "Runner")
            {
                Runner runner = currentEnemy.GetComponent<Runner>();
                if (newTarget == null)
                {
                    newTarget = currentEnemy;
                    bestValue = runner._life;
                }
                else if (bestValue > runner._life)
                {
                    newTarget = currentEnemy;
                }
            }
        }
        if (newTarget != null)
        {
            _currentTarget = newTarget;
        }
    }
    
}
