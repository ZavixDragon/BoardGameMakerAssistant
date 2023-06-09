using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class GamePiece
{
    public string Name;
    public int Width = 300;
    public int Height = 300;
    public string DataSourceId = "";
    public string TableName = "";
    public List<Condition> Filter = new List<Condition>();
    public DataDependent<Color> BackgroundColor = new DataDependent<Color> { Concrete = Color.white };
    public GamePieceType Type = GamePieceType.Board;
    public GameBoard GameBoard = new GameBoard();
    public Card Card = new Card();
    public Token Token = new Token();
    public Die Die = new Die();

    public Element[] Elements => GameBoard.Elements.Concat(Card.Elements).Concat(Token.Elements).Concat(Die.Elements).ToArray();
}