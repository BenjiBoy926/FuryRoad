using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringUtilities
{
    public static string Ordinal(int ordinal)
    {
        string ret = ordinal.ToString();

        if (ordinal == 1)
        {
            ret += "st";
        }
        else if (ordinal == 2)
        {
            ret += "nd";
        }
        else if (ordinal == 3)
        {
            ret += "rd";
        }
        else
        {
            ret += "th";
        }

        return ret;
    }
}
