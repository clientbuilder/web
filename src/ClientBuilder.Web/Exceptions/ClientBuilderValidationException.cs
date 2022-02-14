using System;

namespace ClientBuilder.Web.Exceptions;

public class ClientBuilderValidationException : Exception
{
    public ClientBuilderValidationException(string message)
        : base(message)
    {
    }
}