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
            if (child.gameObject.tag == "Turret")
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
            Vitals.text = "Health: " + runner.Life + "/" + runner.MaxLife;
            Vitals.color = COLOR_VITAL_LIFE;
            Cooldown.text = "Speed: " + runner.BASE_SPEED; // Use real speed value later.
            Value.text = "Bounty: " + runner.Bounty;
        }
        else if (Player.SelectedUnit is Tower)
        {
            Tower tower = (Tower)Player.SelectedUnit;
            PowerManager power = tower.PowerManager;
            

            if (power != null)
            {
                Vitals.text = "Energy: " + power.Energy + "/" + power.Prototype.MaxEnergy;
                Vitals.color = COLOR_VITAL_MANA;
            }
            AttackManager attack = tower.gameObject.GetComponent<AttackManager>();
            Value.text = "Value: " + tower.Prototype.Price;
			if (attack.Prototype != PrototypeDatabase.Active.AttackManagerDefault)
			{
				AttackManagerPrototype attackPrototype = attack.Prototype as AttackManagerPrototype; // Should be get functions instead of doing this here.
				if (attack != null)
				{
					Range.text = "Range: " + UIManager.formatFloat(attackPrototype.Range);
					if (attack.AttackSpeed.modifiedValue == 1)
					{
						Cooldown.text = "Cool: " + UIManager.formatFloat (attack.CooldownRemaining) + "/"
						+ UIManager.formatFloat (attackPrototype.Cooldown);
					}
					else
					{
						Cooldown.text = "Cool: " + UIManager.formatFloat (attack.CooldownRemaining/attack.AttackSpeed.modifiedValue) + "/"
							+ UIManager.formatFloat (attackPrototype.Cooldown/attack.AttackSpeed.modifiedValue);
					}

					Damage.text = "Damage: " + attackPrototype.DamageDisplay;
					if (attack.AbilityPrototype != PrototypeDatabase.Active.AbilityManagerDefault)
					{
						AbilityManagerPrototype abilityPrototype = attack.AbilityPrototype as AbilityManagerPrototype; // Should be get functions instead of doing this here.
						AbilityName.text = abilityPrototype.Name;

						AbilityCooldown.text = "Cool: " + UIManager.formatFloat(attack.AbilityCooldownRemaining) + "/"
							+ UIManager.formatFloat(abilityPrototype.Cooldown);
						
						AbilityEnergycost.text = "Energy: "+abilityPrototype.EnergyCost + "(" +UIManager.formatFloat(abilityPrototype.EnergyCost/abilityPrototype.Cooldown,1)+ ")";
						AbilityEffect.text = "Effect: "+abilityPrototype.EffectDisplay;

						if (abilityPrototype.Trigger == AbilityManagerPrototype.TRIGGER.ON_ATTACK)
						{
							AbilityTrigger.text = "Trigger: On Attack";
						}
						else if(abilityPrototype.Trigger == AbilityManagerPrototype.TRIGGER.ON_ATTACK_OVERRIDE)
						{
							AbilityTrigger.text = "Trigger: Replace Attack";
						}
						else
						{
							AbilityTrigger.text = "Trigger: Constant";
						}
					}
				}

            }
            else
			{
				PowerManagerPrototype powerPrototype = power.Prototype as PowerManagerPrototype;
				// Power Geneator/Transfer Tower.
				Range.text = "Transfer Rate: " + UIManager.formatFloat(powerPrototype.TransferRate);
				Cooldown.text = "Links: " + power.PowerLinksIn.Count + "/" + power.PowerLinksOut.Count;
				if (powerPrototype.isGenerator())
				{
					Damage.text = "Production: " + powerPrototype.PassiveProduction;
				}
				else if (powerPrototype.isConsumer())
				{
					Damage.text = "Consumption: " + powerPrototype.ConsumptionEstimate;
				}
            }

        }
    }
}