using System;
using UnityEngine;

[Serializable]
public class ShapeElement
{
    public ShapeType Type;
    public ConditionedSwitch<Color> Color;
    public int Thickness;
}