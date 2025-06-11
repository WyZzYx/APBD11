using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using APBD11.DTOs;
using APBD11.Interfaces;
using APBD11.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD11.Controllers;

[ApiController]
[Authorize]

    [Route("api/devices")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        
        private readonly ILogger<DeviceController> _logger;


        public DeviceController(IDeviceService deviceService,  ILogger<DeviceController> logger)
        {
            _deviceService = deviceService;
            _logger = logger;

        }
        
        
        
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetAll()
        {
            _logger.LogInformation("GET /api/devices");

            var dtos = await _deviceService.GetAllAsync();
            return Ok(dtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DeviceDto>> GetById(Guid id)
        {
            _logger.LogInformation("GET /api/devices/{id:guid}");

            var dto = await _deviceService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();  
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<DeviceDto>> Create([FromBody] DeviceDto dto)
        {
            _logger.LogInformation("POST /api/devices");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _deviceService.CreateAsync(dto);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.Id },
                    created
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DeviceDto dto)
        {
            
            _logger.LogInformation("PUT /api/devices/{id:guid}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
            {
                return BadRequest(new { error = "Route id and DTO id do not match." });
            }

            try
            {
                await _deviceService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
               return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("DELETE /api/devices/{id:guid}");

            try
            {
                await _deviceService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }