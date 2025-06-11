using APBD11.Data;
using APBD11.Interfaces;
using APBD11.Models;
using Microsoft.EntityFrameworkCore;


namespace APBD11.Services;

public class LookupService : ILookupService
{
    private readonly ApplicationDbContext _ctx;

    public LookupService(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IEnumerable<DeviceType>> GetDeviceTypesAsync()
        => await _ctx.DeviceType
            .AsNoTracking()
            .ToListAsync();

    public async Task<IEnumerable<Role>> GetRolesAsync()
        => await _ctx.Role
            .AsNoTracking()
            .ToListAsync();

    public async Task<IEnumerable<Position>> GetPositionsAsync()
        => await _ctx.Position
            .AsNoTracking()
            .ToListAsync();
}