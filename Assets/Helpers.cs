using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public static class Helpers
{
    /// <summary>
    /// Gets the angle of the x and y coordinates of the given vector in radians
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static double GetAngle(this Vector3 vector)
    {
        return Math.Atan2(vector.y, vector.x);
    }

    /// <summary>
    /// Returns whether the number is within the given range inclusive
    /// </summary>
    /// <param name="numberToCheck"></param>
    /// <param name="bottom">Bottom of the range</param>
    /// <param name="top">Top of the range</param>
    /// <returns></returns>
    public static bool IsInRange<T>(T numberToCheck, T bottom, T top) where T : IComparable<T>
    {
        return (numberToCheck.CompareTo(bottom) >= 0 && numberToCheck.CompareTo(top) <= 0);
    }

    /// <summary>
    /// Converts degrees to radians
    /// </summary>
    /// <param name="degrees"></param>
    /// <returns></returns>
    public static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

    /// <summary>
    /// Converts radians to degrees
    /// </summary>
    /// <param name="radians"></param>
    /// <returns></returns>
    public static double RadiansToDegrees(double radians)
    {
        return radians * 180 / Math.PI;
    }
}
