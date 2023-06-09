using System;
using UnityEngine;

[Serializable]
public class ShapeElement
{
    public ShapeType Type = ShapeType.Rectangle;
    public DataDependent<Color> Color = new DataDependent<Color> { Concrete = UnityEngine.Color.white };
    public int Thickness = 10;
}