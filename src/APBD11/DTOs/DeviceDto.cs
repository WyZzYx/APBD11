using System.ComponentModel.DataAnnotations;

namespace APBD11.DTOs;

public class DeviceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string AdditionalProperties { get; set; } = null!;
    public bool IsEnabled { get; set; }
    public int DeviceTypeId { get; set; }
}