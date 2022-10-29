using System.Collections.Generic;

namespace ClientBuilder.Web.Models;

public class ScaffoldModule
{
    public string Id { get; set; }

    public int Order { get; set; }

    public string Name { get; set; }

    public string ScaffoldTypeName { get; set; }

    public InstanceType Type { get; set; }

    public string ClientId { get; set; }

    public bool Generated { get; set; }

    public string SourceDirectory { get; set; }

    public IEnumerable<ScaffoldModuleFileSystemItem> Files { get; set; }

    public IEnumerable<ScaffoldModuleFileSystemItem> Folders { get; set; }
}