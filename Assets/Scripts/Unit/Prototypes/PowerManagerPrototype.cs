public class PowerManagerPrototype
{
    public int EnergyCap { set; get; }
    public int TransferRate { set; get; }
    public int ConsumptionRate { set; get; } // UI Only, not actually consumption
    public int PassiveProduction { set; get; } 
    // EnergyManagerPrototype(EnergyCap, TransferRate, ConsumptionRate) (Consumption rate is dynamically generated from ability.)

    public PowerManagerPrototype(int iEnergyCap, int iTransferRate)
    {
        EnergyCap       = iEnergyCap;
        TransferRate    = iTransferRate;
        ConsumptionRate = TransferRate / 2;
        PassiveProduction = 0;
    }
    

}