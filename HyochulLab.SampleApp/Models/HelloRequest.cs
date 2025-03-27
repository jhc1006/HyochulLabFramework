using System.ComponentModel.DataAnnotations;

namespace HyochulLab.SampleApp.Models;

public class HelloRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
