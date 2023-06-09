using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionsUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button addNewCondition;
    [SerializeField] private ConditionControlUI conditionPrototype;
    [SerializeField] private Button done;
    
    private List<Condition> _conditions;

    private void Awake()
    {
        addNewCondition.onClick.AddListener(() =>
        {
            var condition = new Condition();
            _conditions.Add(condition);
            Instantiate(conditionPrototype, panel.transform).Init(condition, () => _conditions.Remove(condition));
            Current.SaveProject();
        });
        done.onClick.AddListener(() => gameObject.SetActive(false));
    }
    
    public void Init(List<Condition> conditions)
    {
        _conditions = conditions;
        panel.DestroyAllChildren();
        foreach (var condition in _conditions)
            Instantiate(conditionPrototype, panel.transform).Init(condition, () => _conditions.Remove(condition));
        gameObject.SetActive(true);
    }
}