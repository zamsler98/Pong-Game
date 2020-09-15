using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPaddleControls
{
    /// <summary>
    /// Gets the direction that the paddle should move
    /// </summary>
    /// <returns>1 if paddle moves up else -1</returns>
    float GetDirection();
}

public class PlayerControl : IPaddleControls
{
    string input;
    public PlayerControl(string inputString)
    {
        input = inputString;
    }

    public float GetDirection()
    {
        return Input.GetAxis(input);
    }
}

public class EasyAI : IPaddleControls
{
    public float GetDirection()
    {
        throw new System.NotImplementedException();
    }
}

public class MediumAI : IPaddleControls
{
    public float GetDirection()
    {
        throw new System.NotImplementedException();
    }
}

public class HardAI : IPaddleControls
{
    public float GetDirection()
    {
        throw new System.NotImplementedException();
    }
}