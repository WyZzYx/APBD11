using APBD11.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace APBD11.Controllers;

public class LookupController : ControllerBase
{
    
    private readonly ILookupService _svc;

    public LookupController(ILookupService svc)
    {
        _svc = svc;
    }
    
    [HttpGet("/api/roles")]
    public async Task<IActionResult> GetRoles()
        => Ok(await _svc.GetRolesAsync());

    [HttpGet("/api/positions")]
    public async Task<IActionResult> GetPositions()
        => Ok(await _svc.GetPositionsAsync());
    
    [HttpGet("/api/devices/types")]
    public async Task<IActionResult> GetDeviceTypes()
    {
        var types = await _svc.GetDeviceTypesAsync();
            
        return Ok(types);
    }
}
