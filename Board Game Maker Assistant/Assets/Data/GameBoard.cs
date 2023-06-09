using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class GameBoard
{
    public List<Element> FirstSideElements = new List<Element>();
    public bool IsDoubleSided = false;
    public List<Element> SecondSideElements = new List<Element>();

    public Element[] Elements => FirstSideElements.Concat(SecondSideElements).ToArray();
}