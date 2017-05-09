
using System;

public class PowerManagerPrototype
{
    /// <summary>How much energy the manager can hold.</summary>
    public int MaxEnergy { set; get; }
    /// <summary>How much power can be sent and recieved each tick/summary>
    public int TransferRate { set; get; }
    /// <summary>UI Only, how much is consumed per second. Always half of Transfer Rate</summary>
    public int ConsumptionEstimate { set; get; }
    /// <summary>How much power is produced each tick, for Generators.</summary>
    public int PassiveProduction { set; get; } 
    // EnergyManagerPrototype(EnergyCap, TransferRate, ConsumptionRate) (Consumption rate is dynamically generated from ability.)

    public PowerManagerPrototype(int iMaxEnergy, int iTransferRate)
    {
        MaxEnergy       = iMaxEnergy;
        TransferRate    = iTransferRate;
		ConsumptionEstimate = 0; //TransferRate / 2;
        PassiveProduction = 0;
    }
    /// <summary>
    /// Powered Towers can store power.
    /// They can Send and Recieve Power.
    /// </summary>
    /// <returns></returns>
    public bool isPowered()
    {
        return MaxEnergy != 0 && TransferRate != 0;
    }
    /// <summary>
    /// Generators produce power.
    /// Generators can Send and Recieve Power.
    /// </summary>
    /// <returns></returns>
    public bool isGenerator()
    {
        return PassiveProduction > 0;
    }
    /// <summary>
    /// Consumers use power.
    /// Consumers cannot transfer power out.
    /// </summary>
    /// <returns></returns>
    public bool isConsumer()
    {
        return ConsumptionEstimate != 0;
    }
}