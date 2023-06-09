using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Card
{
    public List<Element> FrontSideElements = new List<Element>();
    public List<Element> BackSideElements = new List<Element>();

    public Element[] Elements => FrontSideElements.Concat(BackSideElements).ToArray();
}