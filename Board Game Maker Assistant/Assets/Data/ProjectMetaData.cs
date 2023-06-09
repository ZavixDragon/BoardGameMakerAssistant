using System;
using System.IO;

[Serializable]
public class ProjectMetaData
{
    public string Name = "My Project";
    public DateTime LastModifiedDate = DateTime.Now;
    public string FilePath;

    public string FullPath => Path.Combine(FilePath, $"{Name}.json");
    public string DirectoryPath => Path.Combine(FilePath, $"{Name}");
}