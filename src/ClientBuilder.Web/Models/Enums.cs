namespace ClientBuilder.Web.Models;

public enum InstanceType
{
    Undefined = 0,
    Web = 1,
    Mobile = 2,
    Desktop = 3,
}

public enum ScaffoldModuleGenerationStatusType
{
    Successful = 1,
    SuccessfulWithErrors = 2,
    Unsuccessful = 3,
}