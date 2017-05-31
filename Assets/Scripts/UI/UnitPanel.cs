using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class UnitPanel : MonoBehaviour {

    public Text Name;

	public Text Vitals;
	public Text Cooldown;
	public Text Range;
	public Text Damage;
	public Text Value;


	public Text AbilityName;
	public Text AbilityCooldown;
	public Text AbilityEnergycost;
	public Text AbilityEffect;
	public Text AbilityTrigger;
    
    public Image Image;
    public Image TurretImage;

    private Color COLOR_VITAL_LIFE = new Color(0.8f, 1.0f, 0.8f);
    private Color COLOR_VITAL_MANA = new Color(0.5f, 0.5f, 1.0f);
    //private Color COLOR_VITAL_OTHER = new Color(0.3f, 0.3f, 0.3f);

    // Use this for initialization
    void Start ()
    {
        Clear();
    }
	
	// Update is called once per frame
	void Update ()
	{
        Clear();
        if (Player.SelectedUnit != null)
        {
            Redraw();
        }
        else
        {
        }
    }

    private void Clear()
    {
		Name.text = "----";
		Vitals.text = "----";
        Image.enabled = false;
        TurretImage.enabled = false;


		Cooldown.text = "----";
		Range.text = "----";
		Damage.text = "----";
		Value.text = "----";

		AbilityName.text = "----";
		AbilityCooldown.text = "----";
		AbilityEnergycost.text = "----";
		AbilityEffect.text = "----";
		AbilityTrigger.text = "----";
        


    }
    

    public void Redraw()
    {

        Name.text = Player.SelectedUnit.gameObject.name;
        
        Image.enabled = true;

        Image.sprite = Player.SelectedUnit.GetComponent<SpriteRenderer>().sprite;

        foreach (Transform child in Player.SelectedUnit.transform)
        {
            if (child.gameObject.tag == "Turret" && child.gameObject.GetComponent<SpriteRenderer>().sprite != null)
            {
                TurretImage.enabled = true;
                TurretImage.sprite = child.GetComponent<SpriteRenderer>().sprite;
                TurretImage.transform.rotation = child.rotation;
                break;
            }
        }

        if (Player.SelectedUnit is Runner)
        {
            Runner runner = (Runner)Player.SelectedUnit;
            Vitals.text = "Health: " + runner._life + "/" + runner._maxLife;
            Vitals.color = COLOR_VITAL_LIFE;
            Cooldown.text = "Speed: " + runner.BASE_SPEED; // Remember to change to mutablespeed later.
            Value.text = "Bounty: " + runner._bounty;

            // Special abilities here later. Divine shield, speed, Feedback.
        }
        else if (Player.SelectedUnit is Tower)
        {
            Tower tower = (Tower)Player.SelectedUnit;
            PowerManager power = tower._powerManager;
            AttackManager attack = tower.gameObject.GetComponent<AttackManager>();
            
            Value.text = "Value: " + tower._prototype._price;


            if (power._prototype._maxEnergy > 0)
            {
                // If it has energy, show it.
                Vitals.text = "Energy: " + power._energy + "/" + power._prototype._maxEnergy;
                Vitals.color = COLOR_VITAL_MANA;
            }


			if (attack._prototype != PrototypeDatabase.Active.AttackManagerDefault)
			{
                
				AttackManagerPrototype attackPrototype = attack._prototype; // Should be get functions instead of doing this here.

				if (attack != null)
				{
                    // If it has an attack and hence isn't the dummy attackManager

					Range.text = "Range: " + UIManager.formatFloat(attackPrototype._range);
					if (attack._attackSpeed.modifiedValue == 1)
					{
						Cooldown.text = "Cool: " + UIManager.formatFloat (attack._cooldownRemaining) + "/"
						+ UIManager.formatFloat (attackPrototype._cooldown);
					}
					else
					{
						Cooldown.text = "Cool: " + UIManager.formatFloat (attack._cooldownRemaining/attack._attackSpeed.modifiedValue) + "/"
							+ UIManager.formatFloat (attackPrototype._cooldown/attack._attackSpeed.modifiedValue);
					}


					Damage.text = "Damage: " + attackPrototype._damageDisplay;
					if (attack._abilityPrototype != PrototypeDatabase.Active.AbilityManagerDefault)
					{
						AbilityManagerPrototype abilityPrototype = attack._abilityPrototype as AbilityManagerPrototype;
						AbilityName.text = abilityPrototype._name;

						AbilityCooldown.text = "Cool: " + UIManager.formatFloat(attack._abilityCooldownRemaining) + "/"
							+ UIManager.formatFloat(abilityPrototype._cooldown);
						
						AbilityEnergycost.text = "Energy: "+abilityPrototype._energyCost + "(" +UIManager.formatFloat(abilityPrototype._energyCost/abilityPrototype._cooldown,1)+ ")";
						AbilityEffect.text = "Effect: "+abilityPrototype._effectDisplay;

                        // These need better descriptions. Don't really want it to even say "Trigger"
						if (abilityPrototype._trigger == AbilityManagerPrototype.TRIGGER.ON_ATTACK)
						{
							AbilityTrigger.text = "Trigger: On Attack"; // Doesn't prevent normal attack
						}
						else if(abilityPrototype._trigger == AbilityManagerPrototype.TRIGGER.ON_ATTACK_OVERRIDE)
						{
							AbilityTrigger.text = "Trigger: Replace Attack"; // Prevents normal attack
						}
						else
						{ 
							AbilityTrigger.text = "Trigger: Constant"; // Whenever cooldown is ready
						}
					}
				}

            }
            else
			{
				PowerManagerPrototype powerPrototype = power._prototype as PowerManagerPrototype;
				// Power Geneator/Transfer Tower.
				Range.text = "Transfer Rate: " + UIManager.formatFloat(powerPrototype._transferRate);
				Cooldown.text = "Links: " + power._powerLinksIn.Count + "/" + power._powerLinksOut.Count;
				if (powerPrototype.isGenerator())
				{
					Damage.text = "Production: " + powerPrototype._passiveProduction;
				}
				else if (powerPrototype.isConsumer())
				{
					Damage.text = "Consumption: " + powerPrototype._consumptionEstimate;
                    // Never displays. No consumes that aren't also attackers.
				}
            }

        }
    }
}