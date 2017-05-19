
using System;

public class PowerManagerPrototype
{
    /// <summary>How much energy the manager can hold.</summary>
    public int _maxEnergy { protected set; get; }
    /// <summary>How much power can be sent and recieved each tick/summary>
    public int _transferRate { protected set; get; }
    /// <summary>UI Only, how much is consumed per second. Always half of Transfer Rate</summary>
    public int _consumptionEstimate { protected set; get; }
    /// <summary>How much power is produced each tick, for Generators.</summary>
    public int _passiveProduction { protected set; get; }
    // EnergyManagerPrototype(EnergyCap, TransferRate, ConsumptionRate) (Consumption rate is dynamically generated from ability.)


    public bool _canSend { protected set; get; }
	public bool _canSendLong { protected set; get; }
	public bool _canRecieve { protected set; get; }

    public PowerManagerPrototype(int iMaxEnergy, int iTransferRate)
    {
        _maxEnergy       = iMaxEnergy;
        _transferRate    = iTransferRate;
		_consumptionEstimate = _transferRate / 2;
        _passiveProduction = 0;
        _canSend = false; // Transfer Towers and Generators Only
        _canSendLong = false; // Transfer Towers only.
        _canRecieve = true; // Everything but wall towers.

    }
    /// <summary>
    /// Powered Towers can store power.
    /// They can Send and Recieve Power.
    /// </summary>
    /// <returns></returns>
    public bool isPowered()
    {
        return _maxEnergy != 0 && _transferRate != 0;
    }
    /// <summary>
    /// Generators produce power.
    /// Generators can Send and Recieve Power.
    /// </summary>
    /// <returns></returns>
    public bool isGenerator()
    {
        return _passiveProduction > 0;
    }
    /// <summary>
    /// Consumers use power.
    /// Consumers cannot transfer power out.
    /// </summary>
    /// <returns></returns>
    public bool isConsumer() // Should this really exist? review
    {
        return _consumptionEstimate != 0;
    }


    public void SetCanSend(bool iFlag)
    {
        _canSend = iFlag;
    }

    public void SetPassiveProduction(int iProduction)
    {
        _passiveProduction = iProduction;
    }

    public void SetCanSendLong(bool iFlag)
    {
        _canSendLong = iFlag;
    }

    public void SetCanRecieve(bool iFlag)
    {
        _canRecieve = iFlag;
    }

}