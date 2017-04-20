using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RanaLib
{
    public class Math
    {
        static public double Round(double iNumber, double iRoundTo)
        {
            if (iRoundTo == 0)
            {
                return iNumber;
            }
            else
            {
                return System.Math.Round(iNumber / iRoundTo) * iRoundTo;
            }
        }
    }
    public class Exception
    {

        public static void Throw(string iString)
        {
            Debug.Log(iString);
        }

    }
}
