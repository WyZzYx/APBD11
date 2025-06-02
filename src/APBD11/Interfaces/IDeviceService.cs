using APBD11.DTOs;
using APBD11.Models;

namespace APBD11.Interfaces;

public interface IDeviceService
{
    Task<IEnumerable<DeviceDto>> GetAllAsync();
    Task<DeviceDto?> GetByIdAsync(Guid id);
    Task<DeviceDto> CreateAsync(DeviceDto dto);
    Task UpdateAsync(Guid id, DeviceDto dto);
    Task DeleteAsync(Guid id);
}