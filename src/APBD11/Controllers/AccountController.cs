using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using APBD11.DTOs;
using APBD11.Interfaces;
using APBD11.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD11.Controllers;

[ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Register([FromBody] AccountRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var account = await _accountService.RegisterAsync(registerDto);
                return CreatedAtAction(nameof(GetById),
                                       new { id = account.Id },
                                       new { account.Id, account.Username, account.RoleId });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

      
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAllAsync();
            var result = accounts.Select(a => new
            {
                a.Id,
                a.Username,
                Role = a.Role.Name
            });
            return Ok(result);
        }

     
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentUserRole != Role.Admin && currentUserId != id)
            {
                return Forbid();
            }

            try
            {
                var account = await _accountService.GetByIdAsync(id);
                return Ok(new
                {
                    account.Id,
                    account.Username,
                    Role = account.Role.Name
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

     
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, [FromBody] AccountRegisterDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);
            var isAdmin = currentUserRole == Role.Admin;

            try
            {
                await _accountService.UpdateAsync(id, updateDto, currentUserId, isAdmin);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _accountService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }