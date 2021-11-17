using System.ComponentModel.DataAnnotations;

namespace PlatformService.DTO;

public class PlatformWriteDTO
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Publisher { get; set; }

    [Required]
    public string Cost { get; set; }
}