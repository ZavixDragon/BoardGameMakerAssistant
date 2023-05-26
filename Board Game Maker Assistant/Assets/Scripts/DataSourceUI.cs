using TMPro;
using UnityEngine;

public class DataSourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private DataSourceTableForm tableForm;
    [SerializeField] private GameObject panel;
    [SerializeField] private DataSourceTableButton buttonPrototype;

    private void OnEnable()
    {
        nameLabel.text = Current.DataSource.Name;
        panel.DestroyAllChildren();
        foreach (var table in Current.DataSource.Tables)
            Instantiate(buttonPrototype, panel.transform).Init(table, tableForm);
    }
}