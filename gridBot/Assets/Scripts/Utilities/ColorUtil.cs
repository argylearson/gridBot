using System;
using UnityEngine;

public static class ColorUtil
{
    public static bool SameColor(Color a, Color b)
    {
        return Math.Abs(a.r - b.r) < .1 && (Math.Abs(a.g - b.g) < .1) && (Math.Abs(a.b - b.b) < .1);
    }
}