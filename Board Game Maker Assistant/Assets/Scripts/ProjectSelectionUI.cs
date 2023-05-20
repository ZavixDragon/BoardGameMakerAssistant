using System.Linq;
using UnityEngine;

public class ProjectSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SelectProjectButton prototype;
    
    private void Awake() => panel.DestroyAllChildren();
    
    private void OnEnable()
    {
        foreach (var project in Current.Projects.List.OrderByDescending(x => x.LastModifiedDate))
            Instantiate(prototype, panel.transform).Init(project);
        Instantiate(prototype, panel.transform);
    }

    private void OnDisable() => panel.DestroyAllChildren();
}
