using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : Joystick
{
    public float GetH()
    {
        return Input.GetAxis("Horizontal"); // Return the horizontal axis value
    }
    public float GetV()
    {
        return Input.GetAxis("Vertical"); // Return the vertical axis value
    }
}