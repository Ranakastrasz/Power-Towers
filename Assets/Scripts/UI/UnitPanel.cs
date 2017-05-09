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

    public Image Image;

    private Color COLOR_VITAL_LIFE = new Color(0.8f, 1.0f, 0.8f);
    private Color COLOR_VITAL_MANA = new Color(0.5f, 0.5f, 1.0f);
    private Color COLOR_VITAL_OTHER = new Color(0.3f, 0.3f, 0.3f);

    // Use this for initialization
    void Start ()
    {
        Clear();
    }
	
	// Update is called once per frame
	void Update ()
    {
        AbilityName.text = "Gold:" + Player.Active.Gold;
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
        Name.text = "";
        Vitals.text = "0/0";
        Image.sprite = null;
        Cooldown.text = "Cool";
        Range.text = "Range";
        Damage.text = "Damage";
        Value.text = "Value";
    }
    

    public void Redraw()
    {

        Name.text = Player.SelectedUnit.gameObject.name;
        Image.sprite = Player.SelectedUnit.gameObject.GetComponent<SpriteRenderer>().sprite;
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
                    Cooldown.text = "Cool: " + UIManager.formatFloat(attack.CooldownRemaining) + "/"
						+ UIManager.formatFloat(attack.Prototype.Cooldown);
                    Damage.text = "Damage: " + attackPrototype.DamageDisplay;
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
        else if (Player.SelectedUnit is Runner_Spawner)
        {
            //Tower tower = (Tower)SelectedUnit;
            Vitals.color = COLOR_VITAL_OTHER;
        }
        else if (Player.SelectedUnit is Runner_Goal)
        {
            //Tower tower = (Tower)SelectedUnit;
            Vitals.color = COLOR_VITAL_OTHER;
        }
    }
}