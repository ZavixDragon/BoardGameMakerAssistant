using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataSourceTableForm : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private Toggle byRow;
    [SerializeField] private TextMeshProUGUI headerLabel;
    [SerializeField] private TMP_InputField header;
    [SerializeField] private TextMeshProUGUI entriesStartAtLabel;
    [SerializeField] private TMP_InputField entriesStartAt;
    [SerializeField] private TMP_Dropdown entryAlias;
    [SerializeField] private TextMeshProUGUI entriesCount;
    [SerializeField] private GameObject propertyPanel;
    [SerializeField] private TextMeshProUGUI propertyPrototype;

    private Table _table;

    public void Init(Table table)
    {
        _table = table;
        if (_table == null)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        nameLabel.text = _table.Name;
        byRow.isOn = _table.Direction == EntryDirection.Row;
        header.text = _table.Header.ToString();
        entriesStartAt.text = _table.EntriesStartAt.ToString();
        Refresh();
    }

    private void Refresh()
    {
        _table.Refresh();
        Current.Project.EnsureValid();
        headerLabel.text = _table.Direction == EntryDirection.Row ? "Header Row:" : "Header Column:";
        entriesStartAtLabel.text = _table.Direction == EntryDirection.Column ? "Row Entries Start At:" : "Column Entries Start At:";
        entriesCount.text = $"Total Entries: {_table.GetEntries().Length.ToString()}";
        _table.SetEntryAlias(_table.EntryAlias);
        entryAlias.options = new[] {""}.Concat(_table.GetHeaders()).Select(x => new TMP_Dropdown.OptionData {text = x}).ToList();
        if (!string.IsNullOrWhiteSpace(_table.EntryAlias))
            entryAlias.value = _table.GetHeaders().FirstIndexOf(x => x == _table.EntryAlias) + 1;
        propertyPanel.DestroyAllChildren();
        foreach (var header in _table.GetHeaders())
            Instantiate(propertyPrototype, propertyPanel.transform).text = header;
    }
    
    private void Awake()
    { 
        gameObject.SetActive(false);
        byRow.onValueChanged.AddListener(x =>
        {
            _table.Direction = x ? EntryDirection.Row : EntryDirection.Column;
            Refresh();
            Current.SaveProject();
        });
        header.onValueChanged.AddListener(x =>
        {
            if (int.TryParse(x, out int header))
            {
                _table.Header = header;
                Refresh();
                Current.SaveProject();
            }
        });
        entriesStartAt.onValueChanged.AddListener(x =>
        {
            if (int.TryParse(x, out int entriesStartAt))
            {
                _table.EntriesStartAt = entriesStartAt;
                Refresh();
                Current.SaveProject();
            }
        });
        entryAlias.onValueChanged.AddListener(x =>
        {
            if (x == 0)
                _table.SetEntryAlias("");
            else
                _table.SetEntryAlias(_table.GetHeaders()[x - 1]);
        });
    }

    private void OnDisable() => gameObject.SetActive(false);
}