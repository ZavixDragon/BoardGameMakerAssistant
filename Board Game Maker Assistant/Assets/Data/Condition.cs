using System;

[Serializable]
public class Condition
{
    public string Property = "";
    public ConditionType ConditionType = ConditionType.Equal;
    public string Value = "";
}