using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConditionControlUI : MonoBehaviour
{
    private static List<TMP_Dropdown.OptionData> _comparisons = new List<TMP_Dropdown.OptionData>
    {
        new TMP_Dropdown.OptionData("="),
        new TMP_Dropdown.OptionData("!=")
    };
    private static List<TMP_Dropdown.OptionData> _numberComparisons = new List<TMP_Dropdown.OptionData>
    {
        new TMP_Dropdown.OptionData("="),
        new TMP_Dropdown.OptionData("!="),
        new TMP_Dropdown.OptionData(">"),
        new TMP_Dropdown.OptionData("<"),
        new TMP_Dropdown.OptionData(">="),
        new TMP_Dropdown.OptionData("<=")
    };
    
    [SerializeField] private TMP_Dropdown property;
    [SerializeField] private TMP_Dropdown comparison;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Button delete;
    
    private Condition _condition;
    private Action _onDelete = () => {};
    
    private void Awake()
    {
        property.onValueChanged.AddListener(x =>
        {
            _condition.Property = Current.Table.GetHeaders()[x];
            RefreshComparisons();
            Current.SaveProject();
        });
        comparison.onValueChanged.AddListener(x =>
        {
            _condition.ConditionType = (ConditionType)x;
            Current.SaveProject();
        });
        input.onValueChanged.AddListener(x =>
        {
            _condition.Value = x;
            Current.SaveProject();
        });
        delete.onClick.AddListener(() =>
        {
            _onDelete();
            Current.SaveProject();
            gameObject.SetActive(false);
        });
    }

    public void Init(Condition condition, Action onDelete)
    {
        _condition = condition;
        _onDelete = onDelete;
        property.options = Current.Table.GetHeaders().Select(x => new TMP_Dropdown.OptionData(x)).ToList();
        property.value = Current.Table.GetHeaders().Contains(_condition.Property) ? Current.Table.GetHeaders().FirstIndexOf(x => x == _condition.Property) : 0;
        _condition.Property = Current.Table.GetHeaders()[property.value];
        RefreshComparisons();
        input.text = _condition.Value;
    }

    private void RefreshComparisons()
    {
        comparison.options = Current.Table.IsNumber(Current.Table.GetHeaders()[property.value])
            ? _numberComparisons
            : _comparisons;
        if (!Current.Table.IsNumber(Current.Table.GetHeaders()[property.value]) && (int) _condition.ConditionType > 1)
            _condition.ConditionType = ConditionType.Equal;
        comparison.value = (int) _condition.ConditionType;
    }       
}