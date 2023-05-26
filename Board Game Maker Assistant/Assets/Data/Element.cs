using System;
using System.Collections.Generic;

[Serializable]
public class Element
{
    public ElementType Type;
    public TransformDetails Transform;
    public List<Condition> IsVisible;
    public TextElement Text;
    public ImageElement Image;
    public ShapeElement Shape;
}