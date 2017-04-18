public class TowerPrototype : Prototype
{
    public TowerPrototype UpgradesTo { get; protected set; }
    public string Name { get; protected set; }
    public int Price { get; protected set; }

    // Also Sprite data

    public AttackManagerPrototype AttackManagerPrototype { get; set; }
    public PowerManagerPrototype PowerManagerPrototype { get; set; }

    public TowerPrototype(string iName, int iPrice)
    {
        Name = iName;
        Price = iPrice;

        AttackManagerPrototype = PrototypeDatabase.Active.AttackManagerDefault;
        PowerManagerPrototype = PrototypeDatabase.Active.PowerManagerDefault;
    }

    public void SetAttackManager(AttackManagerPrototype iAttackPrototype)
    {
        AttackManagerPrototype = iAttackPrototype;
    }

    public void SetPowerManager(PowerManagerPrototype iPowerPrototype)
    {
        PowerManagerPrototype = iPowerPrototype;
    }

    public void SetUpgradesTo(TowerPrototype iUpgradesTo)
    {
        UpgradesTo = iUpgradesTo;
    }

}
