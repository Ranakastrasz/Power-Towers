using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : Entity
{
    public PowerManagerPrototype Prototype { protected set; get; }

    public int Energy { protected set; get; }

    public List<PowerLink> PowerLinksIn = new List<PowerLink>(8);
    public List<PowerLink> PowerLinksOut = new List<PowerLink>(8);

    public int lastEnergy { protected set; get; } // For +- number floaters, I think
    public int totalReceived = 0;
    public int totalSent = 0;
    private int numSlowedByTransferRate = 0;
    private int bottleNeckedCount = 0;
    
        // Consider a better way to do this, prefer not having to handle a list.
        // Possible loop all towers and get their power managers instead?
    protected static List<PowerManager> PowerManagerList = new List<PowerManager>();

    protected override void Start()
    {
        PowerManagerList.Add(this);
        Energy = 0;
        lastEnergy = 0;
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
        if (PowerLinksOut.Count < 2) return;

        PowerLink powerLink = PowerLinksOut[0];

        // Only run if two or more links exist.
        int maxSeen = PowerLinksOut[0].Target.MaxReceivableEnergy();;
        for (int x = 0; x < PowerLinksOut.Count - 1; x++)
        {
            int energy = PowerLinksOut[x + 1].Target.MaxReceivableEnergy();
			// Equal amounts should always be swapped to avoid a single tower hogging a constant +1
            if (energy <= maxSeen)
            {
                PowerLinksOut[x] = PowerLinksOut[x + 1];
                PowerLinksOut[x + 1] = powerLink;
                // keep shifting the best so far towards the end of the list
            }
            else
            {
                powerLink = PowerLinksOut[x + 1];
                maxSeen = energy;
            }
        }

    }

    private int MaxReceivableEnergy()
    {
        return Math.Min(Prototype.MaxEnergy - Energy, Prototype.TransferRate - totalReceived);

    }

    /// <summary>
    /// How much energy this PowerManager can recieve this tick.
    /// Capped by how much more energy it can hold
    /// And by how much it already recieved out of the transfer rate,
    /// </summary>
    /// <returns></returns>
	private int MaxReceivableTransfer()
    {
        
        int transferCap = Prototype.TransferRate - totalReceived;

        return transferCap;
    }



    private int GetEnergyCap()
    {
        int energyCap = Prototype.MaxEnergy - Math.Max(Energy, lastEnergy);
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
        return Math.Min( Energy, Prototype.TransferRate - totalSent);
    }

    public bool CanLinkIn()
    {
        return PowerLinksIn.Count < PowerLink.MAX_LINKS &&  (Prototype.isPowered());
    }
    public bool CanLinkOut()
    {
        return PowerLinksOut.Count < PowerLink.MAX_LINKS && (Prototype.isPowered());
    }
    public void AddLink(PowerManager iTarget, bool longLink)
    {
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
                    if (PowerLinksOut.Find(x => x.Target == iTarget) == null)
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
                        RanaLib.Exception.Throw(this.GetType().Name+".AddLink: Target Exists");
                    }
                }
                else
                {
                    RanaLib.Exception.Throw(this.GetType().Name+".AddLink: iTarget.CanLinkIn() = false");
                }
            }
            else
            {
                RanaLib.Exception.Throw(this.GetType().Name+".AddLink: CanLinkOut = false");
            }
        }
        else
        {
            RanaLib.Exception.Throw(this.GetType().Name+".AddLink: Cannot Link to Self");
        }
        
    }
    
	internal void RemoveLinksIn()
	{
		List<PowerLink> list = new List<PowerLink>();
		list.AddRange(PowerLinksIn);
		foreach (PowerLink powerLink in list)
		{
			RemoveLink(powerLink);
		}
	}
	internal void RemoveLinksOut()
	{
		List<PowerLink> list = new List<PowerLink>();
		list.AddRange(PowerLinksOut);
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
        list.AddRange(PowerLinksIn);
        foreach (PowerLink powerLink in list)
        {
            if (powerLink.LinkRange == PowerLink.LINK_RANGE.LONG)
                RemoveLink(powerLink);
        }
    }
    internal void RemoveLongLinksOut()
    {
        List<PowerLink> list = new List<PowerLink>();
        list.AddRange(PowerLinksOut);
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
        foreach (PowerLink powerLink in PowerLinksOut)
        {
            if (powerLink.Target == iTarget)
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
		if (Energy >= iCost)
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
		if (Energy >= iCost)
		{
			Energy -= iCost;
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
        lastEnergy = Energy;
        totalReceived = 0;
        totalSent = 0;
        numSlowedByTransferRate = 0;

    }

    public void Tick()
    {

        PushEnergy();
        /*foreach (PowerLink powerLink in PowerLinksOut)
        {
            int EnergyToTransfer = Math.Min(Energy, Prototype.TransferRate - totalSent);

            powerLink.PushEnergy(EnergyToTransfer);
        }*/
		if (Energy < 0)
		{
			Energy = Energy;
		}
        Energy = Math.Min(Energy + Prototype.PassiveProduction, Prototype.MaxEnergy);
        
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
        int energyAvailable = Math.Min(Prototype.TransferRate, lastEnergy);
		int transfersRemaining = PowerLinksOut.Count;
        for (int x = 0; x < PowerLinksOut.Count; x++)
        {
            PowerLink powerLink = PowerLinksOut[x];

            //compute amount to send
            int energyToTransfer = energyAvailable / transfersRemaining;

			int targetTransferCap = powerLink.Target.MaxReceivableTransfer();
                
			int targetEnergyCap = powerLink.Target.GetEnergyCap();

            if(targetTransferCap < energyToTransfer || targetEnergyCap < energyToTransfer)
            {
                if (targetTransferCap < targetEnergyCap)
                {
                    energyToTransfer = targetTransferCap;
                    numSlowedByTransferRate++;
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
        powerLink.Target.totalReceived += energyToTransfer;

        totalSent += energyToTransfer;

		powerLink.Target.Energy += energyToTransfer;

        Energy -= energyToTransfer;

		powerLink.lastTransfer = energyToTransfer;
		powerLink.Redraw();
		if (Energy < 0 || Energy > GetEnergyCap() || powerLink.Target.Energy < 0 || powerLink.Target.Energy > powerLink.Target.GetEnergyCap())
		{

		}
    }

    public new void Redraw()
    {
        int energy = Energy;
        int energyChange = energy - lastEnergy;
        
        // Figure this out. Its complex.
        if (totalSent > 0 && energyChange >= 0 && energy > (Prototype.MaxEnergy - Prototype.TransferRate) && numSlowedByTransferRate >= PowerLinksOut.Count / 2)
        {
            bottleNeckedCount = RanaLib.Math.Clamp<int>(0, bottleNeckedCount + 3, 15);
            if (bottleNeckedCount >= 15)
            {
                // EntityManager.CreateTextFloater(gameObject, "|cFFFF0000Bottlenecked|r");
            }
        }
        else
        {
            bottleNeckedCount--;
        }

        if (lastEnergy > 0)
        {
            //showUnitText(.u, "|cFF0000FF+" + I2S(de) + "|r")
        }
        else if (lastEnergy < 0)
        {
            //showUnitText(.u, "|cFFFF00FF" + I2S(de) + "|r")
        }
        else if (lastEnergy == 0 && PowerLinksIn.Count == 0 && Prototype.ConsumptionEstimate > 0)
        {

            //showUnitText(.u, "|cFFFF0000No Power|r")
        }
        else if (PowerLinksOut.Count == 0 && lastEnergy >= MaxSendableEnergy() && Prototype.PassiveProduction > 0)
        {
            //showUnitText(.u, "|cFFFF0000No Target|r")
        }

        if (Prototype.MaxEnergy > 0)
        {
            float shade = (float)lastEnergy / Prototype.MaxEnergy * 0.75f + 0.25f;
            
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
        Prototype = iPrototype;

    }

}
