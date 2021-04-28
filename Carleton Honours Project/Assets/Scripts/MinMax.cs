using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMax 
{
    public float Min { get; private set; }  //Getter and setter
    public float Max { get; private set; }  //Getter and setter

    public MinMax() //Selecting min/max
    {
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    public void AddValue(float v)   //Adds to the min or max
    {
        if (v > Max)
        {
            Max = v;
        }
        if (v < Min)
        {
            Min = v;
        }
    }
}
