using System;
using System.IO;
using System.Linq;
using UnityEngine;

public static class Current
{
    private static Stored<Projects> _storedProjects;
    private static Stored<Project> _storedProject;

    private static Projects _currentProjects = new Projects();
    private static Project _currentProject = new Project();

    public static Projects Projects => _currentProjects;
    public static Project Project => _currentProject;

    public static void Init()
    {
        if (_storedProjects == null)
            _storedProjects = new JsonFileStored<Projects>(Path.Combine(Application.persistentDataPath, "projects.json"), () => new Projects());
        _currentProjects = _storedProjects.Get();
    }
    
    public static void MutateAndSave(Action<Projects> mutate)
    {
        mutate(_currentProjects);
        SaveProjects();
    }

    public static bool TryChangeCurrentProjectName(string name)
    {
        if (_currentProjects.List.Any(x => x.Name.ToLower() == name.ToLower()))
            return false;
        
        _currentProject.MetaData.Name = name;
    }

    public static bool TryChangePath(Project project, string path)
    {
        
    }

    public static void MutateAndSave(Action<Project> mutate)
    {
        mutate(_currentProject);
        SaveProject();
    }

    public static void SelectProject(Func<Projects, ProjectMetaData> selectProject)
    {
        var metaData = selectProject(_currentProjects);
        _storedProject = new JsonFileStored<Project>(, () => new Project());
        _currentProject =  selectProject(_currentProjects);
    }

    private static void SaveProjects()
    {
        if (_storedProjects == null)
            throw new Exception("Can't save before being initialized");
        _storedProjects.Write(_ => _currentProjects);
    }

    private static void SaveProject()
    {
        if (_storedProject == null)
            throw new Exception("Can't save before being initialized");
        _storedProject.Write(_ => _currentProject);
    }
}