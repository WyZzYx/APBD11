using APBD11.Models;

namespace APBD11.Interfaces;

public interface ILookupService
{
    Task<IEnumerable<DeviceType>>  GetDeviceTypesAsync();
    Task<IEnumerable<Role>>        GetRolesAsync();
    Task<IEnumerable<Position>>    GetPositionsAsync();
}