using System;
using System.IO;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using UnityEngine;

public static class Current
{
    private static Stored<GoogleSheetsCredentials> _credentials;
    private static Stored<Projects> _storedProjects;
    private static Stored<Project> _storedProject;

    private static ClientSecrets _currentCredentials;
    private static Projects _currentProjects = new Projects();
    private static ProjectMetaData _currentProjectMetaData = new ProjectMetaData();
    private static Project _currentProject = new Project();
    private static DataSource _currentDataSource;

    public static ClientSecrets GoogleSheetsCredentials => _currentCredentials;
    public static Projects Projects => _currentProjects;
    public static Project Project => _currentProject;
    public static DataSource DataSource => _currentDataSource;

    public static void Init()
    {
        if (_credentials == null)
            _credentials = new JsonFileStored<GoogleSheetsCredentials>(Path.Combine(Application.persistentDataPath, "credentials.json"), () => new GoogleSheetsCredentials());
        var credentials = _credentials.Get();
        _currentCredentials = new ClientSecrets { ClientId = credentials.ClientId, ClientSecret = credentials.ClientSecret };
        if (_storedProjects == null)
            _storedProjects = new JsonFileStored<Projects>(Path.Combine(Application.persistentDataPath, "projects.json"), () => new Projects());
        _currentProjects = _storedProjects.Get();
    }

    public static string TrySaveGoogleSheetsCredentials(string clientId, string clientSecret)
    {
        if (string.IsNullOrWhiteSpace(clientId))
            return "Client ID is required";
        if (string.IsNullOrWhiteSpace(clientSecret))
            return "Client Secret is required";
        _credentials.Write(_ => new GoogleSheetsCredentials { ClientId = clientId, ClientSecret = clientSecret });
        _currentCredentials = new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret };
        return "";
    }
    
    public static void MutateAndSave(Action<Projects> mutateProjects)
    {
        mutateProjects(_currentProjects);
        SaveProjects();
    }
    
    public static void MutateAndSave(Action<Project> mutateProject)
    {
        mutateProject(_currentProject);
        SaveProject();
    }

    public static void SelectProject(ProjectMetaData metaData)
    {
        _currentProjectMetaData = metaData;
        _storedProject = new JsonFileStored<Project>(_currentProjectMetaData.FilePath, () => new Project());
        _currentProject =  _storedProject.Get();
        _currentProject.MetaData = _currentProjectMetaData;
    }
    
    public static void SelectDataSource(DataSource dataSource)
    {
        _currentDataSource = dataSource;
    }

    public static void DeleteProject(ProjectMetaData metaData)
    {
        _currentProjectMetaData = metaData;
        DeleteProjectFiles();
        _currentProjects.List.Remove(_currentProjectMetaData);
        SaveProjects();
    }

    public static string TryChangeCurrentProjectName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Name can't be empty";
        if (_currentProjects.List.Any(x => x.Name.ToLower() == name.ToLower()))
            return "Name is already taken";
        _currentProjectMetaData.Name = name;
        SaveProjects();
        SaveProject();
        return "";
    }

    public static string TryChangeCurrentProjectPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return "Path can't be empty";
        if (_currentProjects.List.Any(x => x.FilePath.ToLower() == path.ToLower()))
            return "Path is already taken";
        _storedProject = new JsonFileStored<Project>(path, () => new Project());
        if (_storedProject.TryWrite(x => _currentProject))
        {
            DeleteProjectFiles();
            _currentProjectMetaData.FilePath = path;
            SaveProjects();
            SaveProject();
            return "";
        }
        else
        {
            _storedProject = new JsonFileStored<Project>(_currentProjectMetaData.FilePath, () => new Project());
            return "Couldn't write to file path provided";
        }
    }

    private static void SaveProjects()
    {
        if (_storedProjects == null)
            throw new Exception("Can't save before being initialized");
        _storedProjects.Write(_ => _currentProjects);
    }

    public static void SaveProject()
    {
        if (_storedProject == null)
            throw new Exception("Can't save before being initialized");
        _currentProjectMetaData.LastModifiedDate = DateTime.Now;
        _storedProject.Write(_ => _currentProject);
    }

    private static void DeleteProjectFiles()
    {
        try
        {
            File.Delete(_currentProjectMetaData.FilePath);
        }
        catch {}
    }
}