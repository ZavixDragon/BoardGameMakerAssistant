using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Table
{
    private string[] _headers;
    private Dictionary<string, string>[] _entries;
    
    public string Name;
    public List<TableRow> RawData;
    public EntryDirection Direction = EntryDirection.Row;
    public int Header = 1;
    public int EntriesStartAt = 2;

    public void Refresh()
    {
        _headers = null;
        _entries = null;
    }

    public string[] GetHeaders()
        => _headers ??= CreateHeaders();

    private string[] CreateHeaders()
    {
        var data = Direction == EntryDirection.Row ? RawData.Select(x => x.RawData).ToList() : GetColumnOrientedData();
        if (data.Count < Header)
            return new string[0];
        return data[Header - 1].ToArray();
    }
    
    public Dictionary<string, string>[] GetEntries()
        => _entries ??= CreateEntries();

    private Dictionary<string, string>[] CreateEntries()
    {
        var data = Direction == EntryDirection.Row ? RawData.Select(x => x.RawData).ToList() : GetColumnOrientedData();
        if (data.Count < Header)
            return new Dictionary<string, string>[0];
        var headers = data[Header - 1];
        var entries = data.Skip(EntriesStartAt - 1).ToArray();
        return entries.Select(x => CreateEntry(headers, x)).ToArray();
    } 

    private List<List<string>> GetColumnOrientedData()
    {
        var data = new List<List<string>>();
        for (int i = 0; i < RawData[0].RawData.Count; i++)
            data.Add(new List<string>());
        for (int row = 0; row < RawData.Count; row++)
            for (int column = 0; column < RawData[0].RawData.Count; column++)
                data[column].Add(RawData[row].RawData[column]);
        return data;
    }

    private Dictionary<string, string> CreateEntry(List<string> headers, List<string> entry)
    {
        var entryMap = new Dictionary<string, string>();
        for (var i = 0; i < headers.Count; i++)
            entryMap[headers[i]] = entry[i];
        return entryMap;
    }
}