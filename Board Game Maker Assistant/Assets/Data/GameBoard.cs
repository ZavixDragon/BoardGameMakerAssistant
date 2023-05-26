using System;
using System.Collections.Generic;

[Serializable]
public class GameBoard
{
    public List<Element> Elements;
    public bool IsDoubleSided;
    public List<Element> DoubleSideElements;
}