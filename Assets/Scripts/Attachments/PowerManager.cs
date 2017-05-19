using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : Entity
{
    public PowerManagerPrototype _prototype { protected set; get; }

    public int _energy { protected set; get; }

    public List<PowerLink> _powerLinksIn = new List<PowerLink>(8);
    public List<PowerLink> _powerLinksOut = new List<PowerLink>(8);

    public int _lastEnergy { protected set; get; } // For +- number floaters, I think
    public int _totalReceived = 0;
    public int _totalSent = 0;
    private int _numSlowedByTransferRate = 0;
    private int _bottleNeckedCount = 0;

    
        // Consider a better way to do this, prefer not having to handle a list.
        // Possible loop all towers and get their power managers instead?
    protected static List<PowerManager> PowerManagerList = new List<PowerManager>();

    protected override void Start()
    {
        PowerManagerList.Add(this);
        _energy = 0;
        _lastEnergy = 0;
        this.Redraw();
    }

    public virtual void OnDestroy()
    {
        PowerManagerList.Remove(this);
    }



    //
    //
    /*private method PerformSortingPass takes nothing returns nothing
            assert(this != 0)
            if .numTransfersOut< 2 then; return; endif

           TowerTransfer transfer; transfer = .transfersOut[0]
           integer maxSeen; maxSeen = transfer.dst.MaxReceivableEnergy()
            integer i
            loopForIntBelow(i, .numTransfersOut-1)
                integer e; e = .transfersOut[i + 1].dst.MaxReceivableEnergy()
                if e <= maxSeen then
                    //keep shifting the best so far towards the end of the list
                    .transfersOut[i] = .transfersOut[i+1]
                    .transfersOut[i+1] = transfer
                else
                    //new best so far
                    transfer = .transfersOut[i + 1]
                    maxSeen = e
                endif
            endloop
        endmethod*/
    /// <summary>
    /// Do a single bubble-sort pass over the transfer list to favor needy towers
    /// Guaranteed to place the neediest tower at the end of the list
    /// </summary>
    private void PerformSortingPass()
    {
        if (_powerLinksOut.Count < 2) return;

        PowerLink powerLink = _powerLinksOut[0];

        // Only run if two or more links exist.
        int maxSeen = _powerLinksOut[0]._target.MaxReceivableEnergy();;
        for (int x = 0; x < _powerLinksOut.Count - 1; x++)
        {
            int energy = _powerLinksOut[x + 1]._target.MaxReceivableEnergy();
			// Equal amounts should always be swapped to avoid a single tower hogging a constant +1
            if (energy <= maxSeen)
            {
                _powerLinksOut[x] = _powerLinksOut[x + 1];
                _powerLinksOut[x + 1] = powerLink;
                // keep shifting the best so far towards the end of the list
            }
            else
            {
                powerLink = _powerLinksOut[x + 1];
                maxSeen = energy;
            }
        }

    }

    private int MaxReceivableEnergy()
    {
        return Math.Min(_prototype._maxEnergy - _energy, _prototype._transferRate - _totalReceived);

    }

    /// <summary>
    /// How much energy this PowerManager can recieve this tick.
    /// Capped by how much more energy it can hold
    /// And by how much it already recieved out of the transfer rate,
    /// </summary>
    /// <returns></returns>
	private int MaxReceivableTransfer()
    {
        
        int transferCap = _prototype._transferRate - _totalReceived;

        return transferCap;
    }



    private int GetEnergyCap()
    {
        int energyCap = _prototype._maxEnergy - Math.Max(_energy, _lastEnergy);
        return energyCap;
            // Why both energy and last energy? I suppose this simulates all the
            // transfers happening at the same time, while still avoiding overflow?
    }
    /// <summary>
    /// How much energy this PowerManager can send this tick.
    /// Capped by how much energy it has
    /// And by how much it already sent out of the transfer rate,
    /// </summary>
    /// <returns></returns>
    private int MaxSendableEnergy()
    {
        return Math.Min( _energy, _prototype._transferRate - _totalSent);
    }

    public bool CanLinkIn()
    {
        return _powerLinksIn.Count < PowerLink.MAX_LINKS &&  (_prototype.isPowered());
    }
    public bool CanLinkOut()
    {
        return _powerLinksOut.Count < PowerLink.MAX_LINKS && (_prototype.isPowered());
    }
    public void AddLink(PowerManager iTarget, bool longLink)
    {
        Vector3 sourcePosition = transform.position;

        Vector3 targetPosition = iTarget.transform.position;

        // Isn't self
        // Has Room
        // Target Has Room
        // Link doesn't already exist.

        // Asserts? Something like it anyway. If x, continue, else break and throw exception.
        if (this != iTarget)
        {
            // Can't link to yoruself
            if (CanLinkOut())
            {
                // Gotta have room for output link
                if (iTarget.CanLinkIn())
                {
                    // And input link on the target
                    if (_powerLinksOut.Find(x => x._target == iTarget) == null)
                    {
                        // Make sure it doesn't exist already. No duplicates.

                        // Check range.
                        if (PowerLink.isInRange(this, iTarget, longLink))
                        {
                            // Always break long links out if you have a link out already.
                            // Only one allowed.
                            if (longLink)
                            {
                                // Long links remove all other links out, and long links in on the target.
                                RemoveLinksOut();
                                iTarget.RemoveLongLinksIn();
                                EntityManager.CreatePowerLink(this, iTarget);
                            }
                            else
                            {
                                // short links cannot exist with long links, in or out.
                                RemoveLongLinksOut();
                                iTarget.RemoveLongLinksIn();
                                EntityManager.CreatePowerLink(this, iTarget);
                            }
                        }

                        
                    }
                    else
                    {
                        EntityManager.CreateFloatingText(targetPosition, "Already Linked", 1.0f, Color.red);
                    }
                }
                else
                {
                    EntityManager.CreateFloatingText(targetPosition, "Link Limit Reached", 1.0f, Color.red);
                }
            }
            else
            {
                EntityManager.CreateFloatingText(sourcePosition, "Link Limit Reached", 1.0f, Color.red);
            }
        }
        else
        {
            EntityManager.CreateFloatingText(sourcePosition, "Cannot Link to Self", 1.0f, Color.red);
        }
        
    }
    
	internal void RemoveLinksIn()
	{
		List<PowerLink> list = new List<PowerLink>();
		list.AddRange(_powerLinksIn);
		foreach (PowerLink powerLink in list)
		{
			RemoveLink(powerLink);
		}
	}
	internal void RemoveLinksOut()
	{
		List<PowerLink> list = new List<PowerLink>();
		list.AddRange(_powerLinksOut);
		foreach (PowerLink powerLink in list)
		{
			RemoveLink(powerLink);
		}
	}
	internal void RemoveLinks()
	{
        RemoveLinksIn();
        RemoveLinksOut();
	}
    
    internal void RemoveLongLinksIn()
    {
        List<PowerLink> list = new List<PowerLink>();
        list.AddRange(_powerLinksIn);
        foreach (PowerLink powerLink in list)
        {
            if (powerLink.LinkRange == PowerLink.LINK_RANGE.LONG)
                RemoveLink(powerLink);
        }
    }
    internal void RemoveLongLinksOut()
    {
        List<PowerLink> list = new List<PowerLink>();
        list.AddRange(_powerLinksOut);
        foreach (PowerLink powerLink in list)
        {
            if (powerLink.LinkRange == PowerLink.LINK_RANGE.LONG)
                RemoveLink(powerLink);
        }
    }
    internal void RemoveLongLinks()
    {
        RemoveLongLinksIn();
        RemoveLongLinksOut();
    }

    public void RemoveLink(PowerLink iLink)
    {
        iLink.remove();
    }

    // Wrapper letting you use the target instead of the link
    public bool RemoveLink(PowerManager iTarget)
    {
        PowerLink link = null;
        foreach (PowerLink powerLink in _powerLinksOut)
        {
            if (powerLink._target == iTarget)
            {
                link = powerLink;
                break;
            }
        }
        if (link != null)
        {
            RemoveLink(link);
            return true;
        }
        else
        {
            return false;
        }
    }

	/// <summary>
	/// Check if enough energy exists to spend
	/// </summary>
	/// <param name="iCost">How much energy to spend</param>
	/// <returns></returns>
	internal bool CanSpendEnergy(int iCost)
	{
		if (_energy >= iCost)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Attempt to spend energy, Return true if enough energy exists
	/// and energy is deducted. False if insufficient energy.
	/// </summary>
	/// <param name="iCost">How much energy to spend</param>
	/// <returns></returns>
	internal bool TrySpendEnergy(int iCost)
	{
		if (_energy >= iCost)
		{
			_energy -= iCost;
			return true;
		}
		else
		{
			return false;
		}
	}


    public static void GlobalTick()
    {
        foreach (PowerManager powerManager in PowerManagerList)
        {
            powerManager.Tick();
        }
    }

    /// <summary>
    /// Clear data for Tick start. Ensures that the stats only apply to the current tick.
    /// </summary>
    public void EndTick()
    {
        _lastEnergy = _energy;
        _totalReceived = 0;
        _totalSent = 0;
        _numSlowedByTransferRate = 0;

    }

    public void Tick()
    {

        PushEnergy();
        /*foreach (PowerLink powerLink in PowerLinksOut)
        {
            int EnergyToTransfer = Math.Min(Energy, Prototype.TransferRate - totalSent);

            powerLink.PushEnergy(EnergyToTransfer);
        }*/
		if (_energy < 0)
		{
			_energy = _energy;
		}
        _energy = Math.Min(_energy + _prototype._passiveProduction, _prototype._maxEnergy);
        
        Redraw();

        EndTick();
    }
    /// <summary>
    ///Transfer energy to other towers for this tick
    ///NOTE: Towers with lots of needed energy are placed towards the end of the
    ///destination list because any energy surplus due to capacitiy early in the list
    ///rolls-over to the later towers in the list.
    /// </summary>
    private void PushEnergy()
    {

        PerformSortingPass();


        // e is Energy
        // n is Numbero f Transfers Out.
        // tt is PowerLink 
        //dst is PowerLink Target.
        // te1 transfer Capacity of reciever
        // te2 is energy capcity of reciever.
        // de = energy to transfer.

        //transfer energy out
        //note: lastEnergy >= current energy because this tower hasn't transfered out yet this tick
        int energyAvailable = Math.Min(_prototype._transferRate, _lastEnergy);
		int transfersRemaining = _powerLinksOut.Count;
        for (int x = 0; x < _powerLinksOut.Count; x++)
        {
            PowerLink powerLink = _powerLinksOut[x];

            //compute amount to send
            int energyToTransfer = energyAvailable / transfersRemaining;

			int targetTransferCap = powerLink._target.MaxReceivableTransfer();
                
			int targetEnergyCap = powerLink._target.GetEnergyCap();

            if(targetTransferCap < energyToTransfer || targetEnergyCap < energyToTransfer)
            {
                if (targetTransferCap < targetEnergyCap)
                {
                    energyToTransfer = targetTransferCap;
                    _numSlowedByTransferRate++;
                    //print(targetTransferCap+" < "+targetEnergyCap+" "+numSlowedByTransferRate);
                }
                else
                {
                    energyToTransfer = targetEnergyCap;
                }
            }

			TransferEnergy(powerLink, energyToTransfer);
            energyAvailable -= energyToTransfer;

            transfersRemaining -= 1;

        }
        
                


    }

    private void TransferEnergy(PowerLink powerLink, int energyToTransfer)
    {
        powerLink._target._totalReceived += energyToTransfer;

        _totalSent += energyToTransfer;

		powerLink._target._energy += energyToTransfer;

        _energy -= energyToTransfer;

		powerLink.lastTransfer = energyToTransfer;
		powerLink.Redraw();
    }

    public new void Redraw()
    {
        int energy = _energy;
        int energyChange = energy - _lastEnergy;
        
        // Figure this out. Its complex.
        if (_totalSent > 0 && energyChange >= 0 && energy > (_prototype._maxEnergy - _prototype._transferRate) && _numSlowedByTransferRate >= _powerLinksOut.Count / 2)
        {
            _bottleNeckedCount = RanaLib.Math.Clamp<int>(0, _bottleNeckedCount + 3, 15);
            if (_bottleNeckedCount >= 15)
            {
                EntityManager.CreateFloatingText(transform.position, "Bottlenecked", 1.0f, Color.red);
                // EntityManager.CreateTextFloater(gameObject, "|cFFFF0000Bottlenecked|r");
            }
        }
        else
        {
            _bottleNeckedCount--;
        }

        if (energyChange > 0)
        { // Energy is increasing
            
            EntityManager.CreateFloatingText(transform.position, energyChange.ToString(), 1.0f, Color.blue);
            //showUnitText(.u, "|cFF0000FF+" + I2S(de) + "|r")
        }
        else if (energyChange < 0)
        { // Energy is Decreasing
            EntityManager.CreateFloatingText(transform.position, energyChange.ToString(), 1.0f, Color.magenta);
            //showUnitText(.u, "|cFFFF00FF" + I2S(de) + "|r")
        }
        else if (_lastEnergy == 0 && _powerLinksIn.Count == 0 && _prototype._consumptionEstimate > 0 && _prototype._passiveProduction == 0)
        { // Consumes power, and cannot recieve any.
            // Change lastEnergy == 0 to lastEnergy < SpellCost, I think

            EntityManager.CreateFloatingText(transform.position, "No Power", 1.0f, Color.red);
            //showUnitText(.u, "|cFFFF0000No Power|r")
        }
        else if (_powerLinksOut.Count == 0 && _lastEnergy >= MaxSendableEnergy() && _prototype._passiveProduction > 0)
        { // Produces Power, but cannot output.
            EntityManager.CreateFloatingText(transform.position, "No Target", 1.0f, Color.red);
            //showUnitText(.u, "|cFFFF0000No Target|r")
        }

        if (_prototype._maxEnergy > 0)
        {
            float shade = (float)_lastEnergy / _prototype._maxEnergy * 0.75f + 0.25f;
            
            SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        
            sprite.material.SetColor("_Color", new Color(shade,shade,shade));

        }

        /*
            if .maxEnergy > 0 then
                real r; r = I2R(.lastEnergy) / .maxEnergy * 0.75 + 0.25
                SetUnitVertexColor(.u, R2I(r*.data.tintRed), R2I(r*.data.tintGreen), R2I(r*.data.tintBlue), 255)
            endif
         */
    }

    /*
      public method Draw takes nothing returns nothing
            assert(this != 0)
            
            integer e; e = .GetEnergy()
            integer de; de = e - .lastEnergy
            if .totalSent > 0 and de >= 0 and e > .maxEnergy - .data.transferPower and .numSlowedByTransferRate >= .numTransfersOut/2 then
                .bottleNeckedCount = between(0, .bottleNeckedCount + 3, 15)
                if .bottleNeckedCount >= 15 then
                    showUnitText(.u, "|cFFFF0000Bottlenecked|r")
                endif
            else
                .bottleNeckedCount -= 1
            endif
            .lastEnergy = e
            .totalReceived = 0
            .totalSent = 0
            .numSlowedByTransferRate = 0

            //show floating text
            if de > 0 then
                showUnitText(.u, "|cFF0000FF+" + I2S(de) + "|r")
            elseif de < 0 then
                showUnitText(.u, "|cFFFF00FF" + I2S(de) + "|r")
            elseif .lastEnergy == 0 and .numTransfersIn == 0 and .data.usageEstimate > 0 then
                showUnitText(.u, "|cFFFF0000No Power|r")
            elseif .numTransfersOut == 0 and .lastEnergy >= .maxEnergy and .data.production > 0 then
                showUnitText(.u, "|cFFFF0000No Target|r")
            endif

            //darken/brighting the tower based on energy level
            if .maxEnergy > 0 then
                real r; r = I2R(.lastEnergy) / .maxEnergy * 0.75 + 0.25
                SetUnitVertexColor(.u, R2I(r*.data.tintRed), R2I(r*.data.tintGreen), R2I(r*.data.tintBlue), 255)
            endif
        endmethod

         */
    /*
      ///Transfer energy to other towers for this tick
        ///NOTE: Towers with lots of needed energy are placed towards the end of the
        ///destination list because any energy surplus due to capacitiy early in the list
        ///rolls-over to the later towers in the list.
        private method PushEnergy takes nothing returns nothing
            assert(this != 0)
            
            .PerformSortingPass()

            //transfer energy out
            //note: lastEnergy >= current energy because this tower hasn't transfered out yet this tick
            integer e; e = IMinBJ(.data.transferPower, .lastEnergy)
            integer n; n = .numTransfersOut
            integer i
            loopForIntBelow(i, .numTransfersOut)
                TowerTransfer tt; tt = .transfersOut[i]
                Tower dst; dst = tt.dst

                //compute amount to send
                integer de; de = e/n
                integer te1; te1 = dst.data.transferPower - dst.totalReceived //transfer capacity of receiver
                integer te2; te2 = dst.maxEnergy - IMaxBJ(dst.GetEnergy(), dst.lastEnergy) //energy capacity of receiver
                if te1 < de or te2 < de then
                    if te1 < te2 then
                        de = te1
                        .numSlowedByTransferRate += 1
                    else
                        de = te2
                    endif
                endif
                
                //transfer
                dst.totalReceived += de
                .totalSent += de
                dst.AdjustEnergy(de)
                .AdjustEnergy(-de)
                tt.Redraw(de)
                e -= de
                
                n -= 1
            endloop
        endmethod
         */
    public void ApplyPrototype(PowerManagerPrototype iPrototype)
    {
        _prototype = iPrototype;

    }

}
