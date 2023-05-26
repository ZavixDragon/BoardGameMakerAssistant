using System;

[Serializable]
public class ConditionWith<T>
{
    public Condition[] Conditions;
    public T Value;
}