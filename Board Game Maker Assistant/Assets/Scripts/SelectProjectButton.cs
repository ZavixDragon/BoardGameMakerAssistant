using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectProjectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI projectName;
    [SerializeField] private TextMeshProUGUI lastModifiedLabel;
    [SerializeField] private Button button;
    [SerializeField] private Button delete;

    private ProjectMetaData _project;
    
    public void Init(ProjectMetaData project) => _project = project;

    private void Start()
    {
        if (_project == null)
        {
            projectName.text = "Start New Project";
            lastModifiedLabel.text = "";
            button.onClick.AddListener(() =>
            {
                Current.MutateAndSave(mutateProjects: projects =>
                {
                    var number = 1;
                    while (Current.Projects.List.Any(x => x.Name == $"My Project {number}") || Current.Projects.List.Any(x => x.FilePath == Path.Combine(Application.persistentDataPath, $"MyProject{number}.json")))
                        number++;
                    var newProject = new ProjectMetaData
                    {
                        Name = $"My Project {number}",
                        FilePath = Path.Combine(Application.persistentDataPath)
                    };
                    projects.List.Add(newProject);
                    Current.SelectProject(newProject);
                    Message.Publish(new NavigateTo(Location.Project));
                });
            });
            delete.gameObject.SetActive(false);
        }
        else
        {
            projectName.text = _project.Name;
            lastModifiedLabel.text = $"Last Modified: {_project.LastModifiedDate:g}";
            button.onClick.AddListener(() =>
            {
                Current.SelectProject(_project);
                Message.Publish(new NavigateTo(Location.Project));
            });
            delete.onClick.AddListener(() =>
            {
                Current.DeleteProject(_project);
                gameObject.SetActive(false);
            });
        }
    }
}