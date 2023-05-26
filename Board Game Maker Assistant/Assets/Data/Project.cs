using System;
using System.Collections.Generic;

[Serializable]
public class Project
{
    public ProjectMetaData MetaData = new ProjectMetaData();
    public List<DataSource> DataSources = new List<DataSource>();
}