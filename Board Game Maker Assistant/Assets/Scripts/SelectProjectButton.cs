using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectProjectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI projectName;
    [SerializeField] private TextMeshProUGUI lastModifiedLabel;
    [SerializeField] private Button button;

    private Project _project;
    
    public void Init(Project project) => _project = project;

    private void Start()
    {
        if (_project == null)
        {
            projectName.text = "Start New Project";
            lastModifiedLabel.text = "";
            button.onClick.AddListener(() =>
            {
                Current.MutateAndSave(x =>
                {
                    var newProject = new Project();
                    x.List.Add(newProject);
                    Current.SelectProject(projects => projects.List.Single(proj => proj == newProject));
                    Message.Publish(new NavigateTo(Location.Project));
                });
                
            });
        }
        else
        {
            projectName.text = _project.Name;
            lastModifiedLabel.text = $"Last Modified: {_project.LastModifiedDate:g}";
            button.onClick.AddListener(() =>
            {
                Current.SelectProject(projects => projects.List.Single(proj => proj == _project));
                Message.Publish(new NavigateTo(Location.Project));
            });
        }
    }
}