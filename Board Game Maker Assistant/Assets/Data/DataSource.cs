using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class DataSource
{
    private Dictionary<string, Table> _tableMap;

    public string Id;
    public string SpreadsheetId;
    public string Name;
    public List<Table> Tables;

    public void Refresh() => _tableMap = null;
    
    public Table GetTable(string tableName) 
        => (_tableMap ??= Tables.ToDictionary(x => x.Name, x => x))[tableName];
}