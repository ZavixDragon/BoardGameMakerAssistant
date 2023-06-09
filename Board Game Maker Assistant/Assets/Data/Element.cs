using System;
using System.Collections.Generic;

[Serializable]
public class Element
{
    public ElementType Type = ElementType.Image;
    public TransformDetails Transform = new TransformDetails();
    public List<Condition> IsVisible = new List<Condition>();
    public TextElement Text = new TextElement();
    public ImageElement Image = new ImageElement();
    public ShapeElement Shape = new ShapeElement();
}