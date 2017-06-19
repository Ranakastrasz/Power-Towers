using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class RunnerData
{
    public static float BASE_SPEED = 1;

    public enum DIFFICULTY
    {
        NOOB,
        ROOKIE,
        HOTSHOT,
        VETERAN,
        ELITE,
        PSYCHO
    }

    //////////
    public static int getRoundRunnerHealth(int iRound, DIFFICULTY iDif)
    {
        double hitpoints;
        if (iRound <= 0)
            return 10;
        if (iDif == DIFFICULTY.NOOB)
        {
            hitpoints = RanaLib.Math.polynom3(iRound, 25, 10, 15, 0);
        }
        else if (iDif == DIFFICULTY.ROOKIE)
        {
            hitpoints = RanaLib.Math.polynom3(iRound, 15, 0, 35, 0);
        }
        else if (iDif == DIFFICULTY.HOTSHOT)
        {
            hitpoints = RanaLib.Math.polynom3(iRound, 50, 21, 28, 1);
        }
        else if (iDif == DIFFICULTY.VETERAN)
        {
            hitpoints = RanaLib.Math.polynom3(iRound, 50, 20, 28, 2);
        }
        else if (iDif == DIFFICULTY.ELITE)
        {
            hitpoints = RanaLib.Math.polynom3(iRound, 40, 80, 0, 6);
        }
        else if (iDif == DIFFICULTY.PSYCHO)
        {
            hitpoints = RanaLib.Math.polynom3(iRound, 64, 128, 0, 10);
        }
        else
        {
            RanaLib.Exception.Throw("Error: Unrecognized difficulty. (getRoundRunnerHealth)");
            return 0;
        }
        if (iRound > Constants.MAX_ROUND)
        {
            hitpoints *= Math.Pow(1.1, iRound - Constants.MAX_ROUND);
        }
        hitpoints = Math.Floor(hitpoints / 25) * 25;
        if (hitpoints < 25)
        {
            hitpoints = 25;
        }
        
        return (int)hitpoints;
    }

    //////////
    public static int getRoundRunnerBounty(int iRound)
    {
        return (int)(Math.Ceiling((double)iRound / 4));//1,1,1,1, 2,2,2,2, 3,3,3,3, ...
    }

    //////////
    public static int getRoundFinishBounty(int iRound)
    {
        return getRoundRunnerBounty(iRound) * 10;
    }

    //////////
    public static int getRoundAccumulatedBounty(int iRound)
    {
        int t = 0;
        for (int x = 0; x < iRound - 1; x++)
        {
            t = t + getRoundRunnerBounty(x + 1) * Constants.SPAWNS_PER_ROUND + getRoundFinishBounty(x + 1);
        }
        return t + Constants.STARTING_GOLD;
    }
    
    //////////
    public static bool isSpecialRound(int iRound)
    {
        return iRound % 5 == 0;
    }
    
    //////////
    /*static bool isSpeedRound(int iRound)
    {
        return isSpecialRound(iRound) && (iRound > Constants.MAX_ROUND || iRound == 5 || iRound == 15 || iRound == 25 || iRound == 30);
    }
    
    //////////
    static bool isFeedbackRound(int iRound)
    {
        return iRound % 5 == 0;
    }
    
    //////////
    static bool isShieldRound(int iRound)
    {
        return iRound % 5 == 0;
    }
    

    //////////
    function isSpeedRound takes integer n returns boolean
        return isSpecialRound(n) and (n > MAX_ROUND or n == 5 or n == 15 or n == 25 or n == 30)
    endfunction

    //////////
    function isFeedbackRound takes integer n returns boolean
        return isSpecialRound(n) and (n > MAX_ROUND or n == 10 or n == 15 or n == 30)
    endfunction

    //////////
    function isShieldRound takes integer n returns boolean
        return isSpecialRound(n) and (n > MAX_ROUND or n == 20 or n == 25 or n == 30)
    endfunction*/
}
