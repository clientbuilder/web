using System;

namespace ClientBuilder.Web.Helpers.Bootstrap;

public class Toast
{
    public Toast()
    {
        this.Id = Guid.NewGuid().ToString().Replace("-", string.Empty);
    }

    public string Id { get; }

    public string Message { get; set; }

    public string Variant { get; set; }

    public bool Executed { get; set; }
}