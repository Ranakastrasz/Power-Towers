public class TowerPrototype : Prototype
{
    public TowerPrototype UpgradesTo { get; protected set; }
    public string Name { get; protected set; }
    public int Price { get; protected set; }

    // Also Sprite data

    public AttackPrototype Attack { get; set; }
    public PowerManagerPrototype PowerManager { get; set; }

    public TowerPrototype(string iName, int iPrice)
    {
        Name = iName;
        Price = iPrice;

        Attack = null;
    }

    public void SetAttack(AttackPrototype iAttack)
    {
        Attack = iAttack;
    }

    public void SetUpgradesTo(TowerPrototype iUpgradesTo)
    {
        UpgradesTo = iUpgradesTo;
    }

}
