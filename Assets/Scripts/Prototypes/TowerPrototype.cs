using UnityEngine;

public class TowerPrototype : Prototype
{

    public TowerPrototype _upgradesTo { get; protected set; }
    public string _name { get; protected set; }
    public int _price { get; protected set; }
    
    public bool _pathable { get; protected set; }

    public Sprite _baseSprite { get; protected set; }
    public Sprite _turretSprite { get; protected set; }

    // Also Sprite data

	public AttackManagerPrototype _attackManagerPrototype { get; set; }
	public AbilityManagerPrototype _abilityManagerPrototype { get; set; }
    public PowerManagerPrototype _powerManagerPrototype { get; set; }

    public TowerPrototype(string iName, int iPrice, Sprite iTurretSprite)
    {
        _name = iName;
        _price = iPrice;
		_baseSprite = PrototypeDatabase.Active.SpriteBase;
        _turretSprite = iTurretSprite;
        _pathable = false;

		_attackManagerPrototype = PrototypeDatabase.Active.AttackManagerDefault;
		_abilityManagerPrototype = PrototypeDatabase.Active.AbilityManagerDefault;
        _powerManagerPrototype = PrototypeDatabase.Active.PowerManagerDefault;
    }

    public void SetAttackManager(AttackManagerPrototype iAttackPrototype)
    {
        _attackManagerPrototype = iAttackPrototype;
    }

	public void SetAbilityManager(AbilityManagerPrototype iAbilityManager)
	{
		_abilityManagerPrototype = iAbilityManager;
	}

	public void SetPowerManager(PowerManagerPrototype iPowerPrototype)
	{
		_powerManagerPrototype = iPowerPrototype;
	}
    
    /// <summary>
    /// The sprite that does not turn, and appears at the bottom of the sprite-stack
    /// </summary>
    /// <param name="iSprite"></param>
    public void SetBaseSprite(Sprite iSprite)
    {
        // For trap towers specifically, use this.
        _baseSprite = iSprite;
    }

    public void SetPatable(bool iPathable)
    {
        _pathable = iPathable;
    }


    public void SetUpgradesTo(TowerPrototype iUpgradesTo)
    {
        _upgradesTo = iUpgradesTo;
    }

}
