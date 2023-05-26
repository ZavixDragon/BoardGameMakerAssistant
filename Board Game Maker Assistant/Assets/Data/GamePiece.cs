using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GamePiece
{
    public string Name;
    public int Width;
    public int Height;
    public string DataSourceId;
    public string TableName;
    public List<Condition> Filter;
    public ConditionedSwitch<Color> BackgroundColor;
    public GamePieceType Type;
    public GameBoard GameBoard;
    public Card Card;
    public Token Token;
    public Die Die;
}