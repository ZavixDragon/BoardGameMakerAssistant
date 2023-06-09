using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataSourceTableButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private Button button;

    public void Init(Table table, DataSourceTableForm form)
    {
        nameLabel.text = table.Name;
        button.onClick.AddListener(() => form.Init(table));
    }
}