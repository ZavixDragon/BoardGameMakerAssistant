using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextInputsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerLabel;
    [SerializeField] private TextMeshProUGUI[] placeholderLabels;
    [SerializeField] private TMP_InputField[] values;
    [SerializeField] private Button submit;
    [SerializeField] private TextMeshProUGUI submitLabel;
    [SerializeField] private Button cancel;
    [SerializeField] private TextMeshProUGUI error;

    private Func<string>[] _getTexts;
    private Func<string[], string> _tryChange;

    public void Init(string header, string[] placeholders, string submitText, Func<string>[] getTexts, Func<string[], string> tryChange)
    {
        headerLabel.text = header;
        for (var i = 0; i < placeholders.Length; i++)
            placeholderLabels[i].text = placeholders[i];
        submitLabel.text = submitText;
        _getTexts = getTexts;
        _tryChange = tryChange;
    } 

    protected void Awake()
    {
        submit.onClick.AddListener(Change);
        cancel.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void OnEnable()
    {
        for (var i = 0; i < values.Length; i++)
            values[i].text = _getTexts[i]();
        error.text = "";
    }

    private void Change()
    {
        error.text = _tryChange(values.Select(x => x.text).ToArray());
        if (string.IsNullOrWhiteSpace(error.text))
            gameObject.SetActive(false);
    }
}