using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RanaLib
{
    public static class Math
    {
        public static double Round(double iNumber, double iRoundTo)
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
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if(val.CompareTo(max) > 0) return max;
            else return val;
        }
        public static double polynom3(double x, int coef0, int coef1, int coef2, int coef3)
        {
            return coef0 + x * (coef1 + x * (coef2 + x * coef3));
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
