using System.ComponentModel.DataAnnotations;

namespace APBD11.Models;

public class Device
{
    public Guid Id { get; set; }
    [Length(1, 50)] [Required] public string Name { get; set; } = null!;
    [Required]
    [Length(1, 100)]
    public string AdditionalProperties { get; set; } = null!;

    public bool IsEnabled { get; set; }
    
    public int DeviceTypeId { get; set; }
    
    public DeviceType DeviceType { get; set; } = null!;
    
    public ICollection<DeviceEmployee> DeviceEmployees { get; set; } = new List<DeviceEmployee>();

}