using UnityEngine;

public class TowerPrototype : Prototype
{

    public TowerPrototype UpgradesTo { get; protected set; }
    public string Name { get; protected set; }
    public int Price { get; protected set; }
    
    public Sprite BaseSprite;
    public Sprite TurretSprite;

    // Also Sprite data

	public AttackManagerPrototype AttackManagerPrototype { get; set; }
	public AbilityManagerPrototype AbilityManagerPrototype { get; set; }
    public PowerManagerPrototype PowerManagerPrototype { get; set; }

    public TowerPrototype(string iName, int iPrice, Sprite iTurretSprite)
    {
        Name = iName;
        Price = iPrice;
        BaseSprite = PrototypeDatabase.Active.WallSprite;
        TurretSprite = iTurretSprite;
        

		AttackManagerPrototype = PrototypeDatabase.Active.AttackManagerDefault;
		AbilityManagerPrototype = PrototypeDatabase.Active.AbilityManagerDefault;
        PowerManagerPrototype = PrototypeDatabase.Active.PowerManagerDefault;
    }

    public void SetAttackManager(AttackManagerPrototype iAttackPrototype)
    {
        AttackManagerPrototype = iAttackPrototype;
    }

	public void SetAbilityManager(AbilityManagerPrototype iAbilityManager)
	{
		AbilityManagerPrototype = iAbilityManager;
	}

	public void SetPowerManager(PowerManagerPrototype iPowerPrototype)
	{
		PowerManagerPrototype = iPowerPrototype;
	}
    
    /// <summary>
    /// The sprite that does not turn, and appears at the bottom of the sprite-stack
    /// </summary>
    /// <param name="iSprite"></param>
    public void SetBaseSprite(Sprite iSprite)
    {
        // For trap towers specifically, use this.
        BaseSprite = iSprite;
    }


    public void SetUpgradesTo(TowerPrototype iUpgradesTo)
    {
        UpgradesTo = iUpgradesTo;
    }

}
