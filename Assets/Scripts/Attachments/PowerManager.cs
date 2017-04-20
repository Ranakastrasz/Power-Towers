using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : Entity
{
    public PowerManagerPrototype Prototype { protected set; get; }

    public int Energy;// { protected set; get; }

    public List<PowerLink> PowerLinksIn = new List<PowerLink>(8);
    public List<PowerLink> PowerLinksOut = new List<PowerLink>(8);

    public int lastEnergy { protected set; get; } // For +- number floaters, I think
    public int totalReceived = 0;
    public int totalSent = 0;
    //private int numSlowedByTransferRate = 0;
    //private int bottleNeckedCount = 0;
    
        // Consider a better way to do this, prefer not having to handle a list.
        // Possible loop all towers and get their power managers instead?
    protected static List<PowerManager> PowerManagerList = new List<PowerManager>();

    protected override void Start()
    {
        PowerManagerList.Add(this);
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
    
    /// <summary>
    /// How much energy this PowerManager can recieve this tick.
    /// Capped by how much more energy it can hold
    /// And by how much it already recieved out of the transfer rate,
    /// </summary>
    /// <returns></returns>
    private int MaxReceivableEnergy()
    {
        return Math.Min(Prototype.MaxEnergy - Energy, Prototype.TransferRate - totalReceived);
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
                            if (longLink)
                            {
                                // Long links remove all other links when placed.
                                RemoveLinks();
                                EntityManager.CreatePowerLink(this, iTarget);
                            }
                            else
                            {
                                RemoveLongLinks();
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
    
    internal void RemoveLinks()
    {
        List<PowerLink> list = new List<PowerLink>();
        list.AddRange(PowerLinksIn);
        list.AddRange(PowerLinksOut);
        foreach (PowerLink powerLink in list)
        {
            RemoveLink(powerLink);
        }
    }

    internal void RemoveLongLinks()
    {
        List<PowerLink> list = new List<PowerLink>();
        list.AddRange(PowerLinksIn);
        list.AddRange(PowerLinksOut);
        foreach (PowerLink powerLink in list)
        {
            if (powerLink.LinkRange == PowerLink.LINK_RANGE.LONG)
                RemoveLink(powerLink);
        }
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
    /// Attempt to spend energy, Return true if enough energy exists
    /// and energy is deducted. False if insufficient energy.
    /// </summary>
    /// <param name="iCost">How much energy to spend</param>
    /// <returns></returns>
    internal bool TrySpendEnergy(int iCost)
    {
        if (Energy > iCost)
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
            powerManager.StartTick();
        }
        foreach (PowerManager powerManager in PowerManagerList)
        {
            powerManager.Tick();
        }
    }

    /// <summary>
    /// Clear data for Tick start. Ensures that the stats only apply to the current tick.
    /// </summary>
    public void StartTick()
    {
        totalSent = 0;
        totalReceived = 0;
        lastEnergy = Energy;
    }

    public void Tick()
    {

        foreach (PowerLink powerLink in PowerLinksOut)
        {
            int EnergyToTransfer = Math.Min(Energy, Prototype.TransferRate - totalSent);

            powerLink.PushEnergy(EnergyToTransfer);
        }

        Energy = Math.Min(Energy + Prototype.PassiveProduction, Prototype.MaxEnergy);
        

    }
    /// <summary>
    ///Transfer energy to other towers for this tick
    ///NOTE: Towers with lots of needed energy are placed towards the end of the
    ///destination list because any energy surplus due to capacitiy early in the list
    ///rolls-over to the later towers in the list.
    /// </summary>
    /*private void PushEnergy()
    {
            
        PerformSortingPass()

        //transfer energy out
        //note: lastEnergy >= current energy because this tower hasn't transfered out yet this tick
        int e = Math.Min(.data.transferPower, .lastEnergy);
        int n = .numTransfersOut;
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
    }*/

    public override void Redraw()
    {
        // Create Floaters.
    }
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
