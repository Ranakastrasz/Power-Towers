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
    //public bool farTransfering { protected set; get; }
    
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



    //Do a single bubble-sort pass over the transfer list to favor needy towers
    //Guaranteed to place the neediest tower at the end of the list
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


    public void AddLink(PowerManager iTarget, bool longLink)
    {
        if (PowerLinksOut.Count < PowerLink.MAX_LINKS && iTarget.PowerLinksIn.Count < PowerLink.MAX_LINKS)
        {
            // Valid to link.
            // Create a Link
            GameObject obj = EntityManager.CreatePowerLink(this, iTarget);
            /*PowerLink link = */obj.GetComponent<PowerLink>();
            

        }
        else
        {
            // Throw LinkError.
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

        Energy = Math.Min(Energy + Prototype.PassiveProduction, Prototype.EnergyCap);
        

    }

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
