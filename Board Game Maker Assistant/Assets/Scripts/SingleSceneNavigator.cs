using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleSceneNavigator : OnMessage<NavigateTo>
{
    [SerializeField] private GameObject projectSelection;
    [SerializeField] private GameObject project;
    [SerializeField] private Button back;

    private Dictionary<Location, GameObject> _locationObjectMap;
    private Location _location;

    private void Awake()
    {
        projectSelection.SetActive(true);
        project.SetActive(false);
        back.onClick.AddListener(NavigateBack);
        _locationObjectMap = new Dictionary<Location, GameObject>
        {
            {Location.ProjectSelection, projectSelection},
            {Location.Project, project},
        };
        ChangeLocation(Location.ProjectSelection);
    }

    protected override void Execute(NavigateTo msg) => ChangeLocation(Location.Project);

    private void NavigateBack()
    {
        if (_location == Location.ProjectSelection)
            Application.Quit();
        else if (_location == Location.Project)
            ChangeLocation(Location.ProjectSelection);
    }

    private void ChangeLocation(Location location)
    {
        _locationObjectMap[_location].SetActive(false);
        _location = location;
        _locationObjectMap[_location].SetActive(true);
    }
}