using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Project
{
    public ProjectMetaData MetaData = new ProjectMetaData();
    public List<DataSource> DataSources = new List<DataSource>();
    public List<GamePiece> Pieces = new List<GamePiece>();

    public GamePiece AddGamePiece()
    {
        var i = 1;
        while (Pieces.Any(x => x.Name.ToLower() == $"Game Piece {i}"))
            i++;
        var gamePiece = new GamePiece {Name = $"Game Piece {i}"};
        Pieces.Add(gamePiece);
        return gamePiece;
    }
    
    public bool TrySetGamePieceName(GamePiece piece, string name)
    {
        if (Pieces.Any(x => x.Name.ToLower() == name.ToLower()))
            return false;
        piece.Name = name;
        return true;
    }

    public void EnsureValid()
    {
        foreach (var piece in Pieces)
        {
            if (!string.IsNullOrWhiteSpace(piece.DataSourceId) && DataSources.All(x => x.Id != piece.DataSourceId))
            {
                ChangeDataSource(piece, "");
                continue;
            }
            var dataSource = string.IsNullOrWhiteSpace(piece.DataSourceId)
                ? null
                : DataSources.First(x => x.Id == piece.DataSourceId);
            if (dataSource == null || dataSource.Tables.Count == 0)
            {
                ChangeTable(piece, "");
                continue;
            }
            if (dataSource.Tables.All(x => x.Name != piece.TableName))
            {
                ChangeTable(piece, dataSource.Tables[0].Name);
                continue;
            }

            var table = dataSource.Tables.First(x => x.Name == piece.TableName);
            piece.Filter.RemoveAll(condition => !IsValidCondition(table, condition));
            piece.BackgroundColor.Cases.ForEach(x => x.Conditions.RemoveAll(condition => !IsValidCondition(table, condition)));
            piece.BackgroundColor.Cases.RemoveAll(x => x.Conditions.Count == 0);
            foreach (var element in piece.Elements)
            {
                element.IsVisible.RemoveAll(condition => !IsValidCondition(table, condition));
                element.Text.Content.Cases.ForEach(x => x.Conditions.RemoveAll(condition => !IsValidCondition(table, condition)));
                element.Text.Content.Cases.RemoveAll(x => x.Conditions.Count == 0);
                element.Text.Color.Cases.ForEach(x => x.Conditions.RemoveAll(condition => !IsValidCondition(table, condition)));
                element.Text.Color.Cases.RemoveAll(x => x.Conditions.Count == 0);
                element.Image.ImagePath.Cases.ForEach(x => x.Conditions.RemoveAll(condition => !IsValidCondition(table, condition)));
                element.Image.ImagePath.Cases.RemoveAll(x => x.Conditions.Count == 0);
                element.Shape.Color.Cases.ForEach(x => x.Conditions.RemoveAll(condition => !IsValidCondition(table, condition)));
                element.Shape.Color.Cases.RemoveAll(x => x.Conditions.Count == 0);
            }
        }
    }
    
    public void ChangeDataSource(GamePiece piece, string dataSourceId)
    {
        piece.DataSourceId = dataSourceId;
        var dataSource = string.IsNullOrWhiteSpace(piece.DataSourceId) ? null : DataSources.First(x => x.Id == piece.DataSourceId);
        ChangeTable(piece, dataSource == null || dataSource.Tables.Count == 0 ? "" : dataSource.Tables[0].Name);
    }

    public void ChangeTable(GamePiece piece, string tableName)
    {
        piece.TableName = tableName;
        piece.Filter.Clear();
        piece.BackgroundColor.Cases.Clear();
        foreach (var element in piece.Elements)
        {
            element.IsVisible.Clear();
            element.Text.Content.Cases.Clear();
            element.Text.Color.Cases.Clear();
            element.Image.ImagePath.Cases.Clear();
            element.Shape.Color.Cases.Clear();
        }
    }
    
    private bool IsValidCondition(Table table, Condition condition) 
        => table.GetHeaders().Any(header => header == condition.Property) && ((int)condition.ConditionType <= 1 || table.IsNumber(condition.Property));
}