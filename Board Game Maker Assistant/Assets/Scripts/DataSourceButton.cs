using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataSourceButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dataSourceName;
    [SerializeField] private Button edit;
    [SerializeField] private Button refresh;
    [SerializeField] private Button delete;

    private DataSource _dataSource;

    public void Init(DataSource dataSource) => _dataSource = dataSource;

    private void Start()
    {
        if (_dataSource == null)
        {
            dataSourceName.text = "Add Data Source";
            edit.onClick.AddListener(() => Message.Publish(new AddDataSource()));
            refresh.gameObject.SetActive(false);
            delete.gameObject.SetActive(false);
        }
        else
        {
            dataSourceName.text = _dataSource.Name;
            edit.onClick.AddListener(() =>
            {
                Current.SelectDataSource(_dataSource);
                Message.Publish(new NavigateTo(Location.DataSource));
            });
            refresh.onClick.AddListener(() =>
            {
                Message.Publish(new RefreshDataSource(_dataSource));
            });
            delete.onClick.AddListener(() =>
            {
                Current.MutateAndSave(x =>
                {
                    x.DataSources.Remove(_dataSource);
                    x.EnsureValid();
                });
                Message.Publish(new DataSourcesUpdated());
            });
        }
    }
}