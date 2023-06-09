using System.Collections.Generic;
using System.Linq;

public class Die
{
    public List<Element> SideOne = new List<Element>();
    public List<Element> SideTwo = new List<Element>();
    public List<Element> SideThree = new List<Element>();
    public List<Element> SideFour = new List<Element>();
    public List<Element> SideFive = new List<Element>();
    public List<Element> SideSix = new List<Element>();
    
    public Element[] Elements => SideOne.Concat(SideTwo).Concat(SideThree).Concat(SideFour).Concat(SideFive).Concat(SideSix).ToArray();
}