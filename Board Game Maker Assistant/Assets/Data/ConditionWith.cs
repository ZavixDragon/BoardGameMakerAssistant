using System;
using System.Collections.Generic;

[Serializable]
public class ConditionWith<T>
{
    public List<Condition> Conditions = new List<Condition>();
    public T Value;
}