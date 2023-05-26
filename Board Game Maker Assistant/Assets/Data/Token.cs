using System;
using System.Collections.Generic;

[Serializable]
public class Token
{
    public List<Element> FrontSideElements;
    public List<Element> BackSideElements;
    public bool SameOnBothSides;
}