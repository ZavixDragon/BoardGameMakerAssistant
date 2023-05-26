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
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class ProjectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI projectName;
    [SerializeField] private TextMeshProUGUI projectPath;
    [SerializeField] private Button editProjectNameButton;
    [SerializeField] private Button editProjectPathButton;
    [SerializeField] private TextInputsUI editProjectName;
    [SerializeField] private TextInputsUI editProjectPath;
    [SerializeField] private TextInputsUI addDataSource;
    [SerializeField] private TextInputsUI setGoogleSheetsCredentials;

    private void Awake()
    {
        editProjectNameButton.onClick.AddListener(() => editProjectName.gameObject.SetActive(true));
        editProjectPathButton.onClick.AddListener(() => editProjectPath.gameObject.SetActive(true));
        editProjectName.Init("Edit Project Name", new [] {"Project Name"}, "Change", new Func<string>[] {() => Current.Project.MetaData.Name}, x =>
        {
            var result = Current.TryChangeCurrentProjectName(x[0]);
            if (string.IsNullOrWhiteSpace(result))
                projectName.text = Current.Project.MetaData.Name;
            return result;
        });
        editProjectPath.Init("Edit Project Path", new [] {"File Path"}, "Change", new Func<string>[] {() => Current.Project.MetaData.FilePath}, x =>
        {
            var result = Current.TryChangeCurrentProjectPath(x[0]);
            if (string.IsNullOrWhiteSpace(result))
                projectPath.text = Current.Project.MetaData.FilePath;
            return result;
        });
        addDataSource.Init("Add Data Source", new [] {"Google Spreadsheet ID"}, "Add", new Func<string>[] {() => ""}, x =>
        {
            var error = TryGetDataSource(x[0], out var dataSource);
            if (string.IsNullOrWhiteSpace(error))
            {
                Current.MutateAndSave(project => project.DataSources.Add(dataSource));
                Message.Publish(new DataSourcesUpdated());
            }
            return error;
        });
        setGoogleSheetsCredentials.Init("Save Google Client Credentials", new [] {"Client ID", "Client Secret"}, "Save", new Func<string>[] {() => "", () => ""}, 
            x => Current.TrySaveGoogleSheetsCredentials(x[0], x[1]));
    }

    private void OnEnable()
    {
        Message.Subscribe<AddDataSource>(_ => addDataSource.gameObject.SetActive(true), this);
        Message.Subscribe<RefreshDataSource>(msg => RefreshDataSource(msg.DataSource), this);
        projectName.text = Current.Project.MetaData.Name;
        projectPath.text = Current.Project.MetaData.FilePath;
        editProjectName.gameObject.SetActive(false);
        editProjectPath.gameObject.SetActive(false);
        addDataSource.gameObject.SetActive(false);
        setGoogleSheetsCredentials.gameObject.SetActive(false);
    }

    private void OnDisable() => Message.Unsubscribe(this);

    private void RefreshDataSource(DataSource dataSource)
    {
        var error = TryGetDataSource(dataSource.SpreadsheetId, out var updatedDataSource);
        if (!string.IsNullOrWhiteSpace(error))
            return;
        dataSource.Name = updatedDataSource.Name;
        dataSource.Tables = updatedDataSource.Tables.Select(updatedTable =>
        {
            var outdatedTable = dataSource.Tables.FirstAsMaybe(table => table.Name.ToLower() == updatedTable.Name.ToLower());
            if (outdatedTable.IsMissing)
                return updatedTable;
            return new Table
            {
                Name = updatedTable.Name,
                Direction = outdatedTable.Value.Direction,
                Header = outdatedTable.Value.Header,
                EntriesStartAt = outdatedTable.Value.EntriesStartAt,
                RawData = updatedTable.RawData
            };
        }).ToList();
        dataSource.Refresh();
        Current.SaveProject();
        Message.Publish(new DataSourcesUpdated());
    }
    
    private string TryGetDataSource(string spreadsheetId, out DataSource dataSource)
    {
        dataSource = null;
        try
        {
            var secrets = Current.GoogleSheetsCredentials;
            if (string.IsNullOrWhiteSpace(secrets.ClientId) || string.IsNullOrWhiteSpace(secrets.ClientSecret))
            {
                setGoogleSheetsCredentials.gameObject.SetActive(true);
                return "Missing Client Credentials";   
            }
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    secrets,
                    new[] {SheetsService.Scope.SpreadsheetsReadonly},
                    "Board Game Maker Assistant",
                    CancellationToken.None,
                    new FileDataStore("token.json", true)).Result,
                ApplicationName = "Board Game Maker Assistant",
            });
            Spreadsheet sheets = service.Spreadsheets.Get(spreadsheetId).Execute();
            dataSource = new DataSource
            {
                Id = Guid.NewGuid().ToString(),
                Name = sheets.Properties.Title,
                SpreadsheetId = spreadsheetId,
                Tables = sheets.Sheets.Select(sheet =>
                {
                    var values = service.Spreadsheets.Values.Get(spreadsheetId, $"{sheet.Properties.Title}!A1:ZZ99999").Execute();
                    return new Table
                    {
                        Name = sheet.Properties.Title,
                        RawData = values.Values.Select(rows => new TableRow { RawData = rows.Select(cell => cell.ToString()).ToList() }).ToList(),
                        Direction = EntryDirection.Row,
                        Header = 1,
                        EntriesStartAt = 2
                    };
                }).ToList()
            };
            return "";
        }
        catch (Exception ex)
        {
            setGoogleSheetsCredentials.gameObject.SetActive(true);
            return ex.Message;
        }
    }
}