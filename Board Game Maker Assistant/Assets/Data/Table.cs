using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Table
{
    private string[] _headers;
    private Dictionary<string, string>[] _entries;
    private Dictionary<string, bool> _isNumber;

    public string Name;
    public List<TableRow> RawData = new List<TableRow>();
    public EntryDirection Direction = EntryDirection.Row;
    public int Header = 1;
    public int EntriesStartAt = 2;
    public string EntryAlias;

    public void Refresh()
    {
        _headers = null;
        _entries = null;
        _isNumber = null;
    }

    public string[] GetHeaders()
    {
        if (_headers == null)
            GetEntries();
        return _headers;
    }

    public bool IsNumber(string header)
    {
        if (_isNumber == null)
            GetEntries();
        return _isNumber[header];
    }

    public Dictionary<string, string>[] GetEntries()
        => _entries ??= CreateEntries();

    private Dictionary<string, string>[] CreateEntries()
    {
        var data = Direction == EntryDirection.Row ? RawData.Select(x => x.RawData).ToList() : GetColumnOrientedData();
        if (data.Count < Header)
            return new Dictionary<string, string>[0];
        _headers = data[Header - 1].ToArray();
        _isNumber = _headers.ToDictionary(x => x, _ => true);
        var entries = data.Skip(EntriesStartAt - 1).ToArray();
        return entries.Select(x => CreateEntry(x)).ToArray();
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

    private Dictionary<string, string> CreateEntry(List<string> entry)
    {
        var entryMap = new Dictionary<string, string>();
        for (var i = 0; i < _headers.Length; i++)
        {
            entryMap[_headers[i]] = entry[i];
            if (_isNumber[_headers[i]] && !decimal.TryParse(entry[i], out var _))
                _isNumber[_headers[i]] = false;
        }
        return entryMap;
    }

    public void SetEntryAlias(string entryAlias)
    {
        if (GetHeaders().Contains(entryAlias))
            EntryAlias = entryAlias;
        else if (GetHeaders().Any(x => x.ToLower() == "name"))
            EntryAlias = GetHeaders().First(x => x.ToLower() == "name");
        else
            EntryAlias = "";
    }
}