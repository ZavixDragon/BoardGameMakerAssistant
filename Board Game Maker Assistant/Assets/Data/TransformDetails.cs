using System;

[Serializable]
public class TransformDetails
{
    public HorizontalTransformAlignment HorizontalAlignment = HorizontalTransformAlignment.Center;
    public VerticalTransformAlignment VerticalAlignment = VerticalTransformAlignment.Center;
    public int X = 0;
    public int Y = 0;
    public int Width = 300;
    public int Height = 300;
    public int Top = 0;
    public int Left = 0;
    public int Right = 0;
    public int Bottom = 0;
}
