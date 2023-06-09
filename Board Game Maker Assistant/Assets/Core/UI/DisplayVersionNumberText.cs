using TMPro;
using UnityEngine;

public sealed class DisplayVersionNumberText : MonoBehaviour
{
    [SerializeField] private string prefix;
    [SerializeField] private StringReference value;
    [SerializeField, NoLocalizationNeeded] private TextMeshProUGUI text;

    private void Awake() => text.text = $"{prefix}{value}";
}
