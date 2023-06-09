using System;
using System.Collections.Generic;

[Serializable]
public class DataDependent<T>
{
    public DataDependentType Type = DataDependentType.Concrete;
    public T Concrete;
    public string Property = "";
    public List<ConditionWith<T>> Cases = new List<ConditionWith<T>>();
}