using System;

[Serializable]
public class ConditionedSwitch<T>
{
    public T Default;
    public ConditionWith<T>[] Cases;
}