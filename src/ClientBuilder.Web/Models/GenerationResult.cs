using System.Collections.Generic;

namespace ClientBuilder.Web.Models;

public class GenerationResult
{
    public IEnumerable<string> Errors { get; set; }

    public ScaffoldModuleGenerationStatusType GenerationStatus { get; set; }

    public static GenerationResult UnsuccessfulResult(string message) =>
        new ()
        {
            GenerationStatus = ScaffoldModuleGenerationStatusType.Unsuccessful,
            Errors = new List<string> { message },
        };
}