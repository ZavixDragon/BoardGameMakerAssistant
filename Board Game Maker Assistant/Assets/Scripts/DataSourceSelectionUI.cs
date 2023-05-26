using UnityEngine;

public class DataSourceSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private DataSourceButton prototype;
    
    private void OnEnable()
    {
        Message.Subscribe<DataSourcesUpdated>(_ => Refresh(), this);
        Refresh();
    }

    private void OnDisable() => Message.Unsubscribe(this);

    private void Refresh()
    {
        panel.DestroyAllChildren();
        foreach (var dataSource in Current.Project.DataSources)
            Instantiate(prototype, panel.transform).Init(dataSource);
        Instantiate(prototype, panel.transform);
    }
}