using System;
using System.ComponentModel.DataAnnotations;
using ClientBuilder.Web.Models;

namespace ClientBuilder.Web.DataTransferObjects;

public class ApplicationDto
{
    public ApplicationDto()
    {
    }

    public ApplicationDto(Application application)
    {
        this.Id = application.Id;
        this.Name = application.Name;
        this.Url = application.Url;
    }

    public Guid Id { get; set; }

    [Required(ErrorMessage = "Application name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Application URL is required")]
    [Url(ErrorMessage = "Entered URL is invalid")]
    public string Url { get; set; }
}