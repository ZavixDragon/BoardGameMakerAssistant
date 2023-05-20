using System;

//Treat this as mutable
[Serializable]
public class ProjectMetaData
{
    public string Name = "My Project";
    public DateTime LastModifiedDate = DateTime.Now;
    public string FilePath;
}