using APBD11.Data;
using APBD11.Interfaces;
using APBD11.Models;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD11.Controllers;

public class AuthController : ControllerBase
{
    
    private readonly ApplicationDbContext _dbContext;
    private readonly ITokenService _tokenService;
    private readonly PasswordHasher<Account> _passwordHasher = new();

    public AuthController(ApplicationDbContext dbContext, ITokenService tokenService)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
    }

    [HttpPost("/api/auth/login")]
    public async Task<IActionResult> Auth(Account account, CancellationToken cancellationToken)
    {
        var found = await _dbContext.Account.Include(a => a.Role).FirstOrDefaultAsync(a => a.Username == account.Username && a.Password == account.Password, cancellationToken);
        if (found == null)
        {
            return Unauthorized();
        }
        var verificationResult = _passwordHasher.VerifyHashedPassword(found, found.Password, account.Password);
        
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }

        var tokens = new
        {
            AccessToken = _tokenService.GenerateToken(found.Username, found.Role.Name)
        };
        return Ok(tokens);
    }
}