using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APBD11.Data;
using APBD11.DTOs;
using APBD11.Interfaces;
using APBD11.Models;
using DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace APBD11.Services;

 public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AccountService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Account> RegisterAsync(AccountRegisterDto registerDto)
        {
            if (await _context.Account.AnyAsync(a => a.Username == registerDto.Username))
            {
                throw new InvalidOperationException($"Username '{registerDto.Username}' is already taken.");
            }

            var account = new Account
            {
                Username = registerDto.Username,
                Password = registerDto.Password,
                RoleId   = registerDto.RoleId
            };

            _context.Account.Add(account);
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto authDto)
        {
            var account = await _context.Account
                                        .Include(a => a.Role)
                                        .SingleOrDefaultAsync(a =>
                                            a.Username == authDto.Username &&
                                            a.Password == authDto.Password);

            if (account == null)
            {
                return null;
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Role, account.Role.Name)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(
                double.Parse(_configuration["Jwt:ExpireMinutes"])
            );

            var token = new JwtSecurityToken(
                issuer:            _configuration["Jwt:Issuer"],
                audience:          _configuration["Jwt:Audience"],
                claims:            claims,
                expires:           expires,
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDto
            {
                Token     = jwt,
                Username  = account.Username,
                Role      = account.Role.Name,
                ExpiresAt = expires
            };
        }

        public async Task<Account> GetByIdAsync(string id)
        {
            var account = await _context.Account
                                        .Include(a => a.Role)
                                        .SingleOrDefaultAsync(a => a.Id.Equals(id));

            if (account == null)
            {
                throw new KeyNotFoundException("Account not found.");
            }

            return account;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Account
                                 .Include(a => a.Role)
                                 .ToListAsync();
        }

        public async Task UpdateAsync(string id, AccountRegisterDto updateDto, string currentUserId, bool isAdmin)
        {
            if (!Guid.TryParse(id, out var guidId))
                throw new KeyNotFoundException("Invalid GUID format.");

            var account = await _context.Account.FindAsync(guidId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found.");
            }

            if (!isAdmin && account.Id.ToString() != currentUserId)
            {
                throw new UnauthorizedAccessException("You can update only your own account.");
            }

            if (account.Username != updateDto.Username)
            {
                if (await _context.Account.AnyAsync(a => 
                    a.Username == updateDto.Username && a.Id.Equals(id)))
                {
                    throw new InvalidOperationException($"Username '{updateDto.Username}' is already taken.");
                }

                account.Username = updateDto.Username;
            }

            account.Password = updateDto.Password;

            if (isAdmin)
            {
                account.RoleId = updateDto.RoleId;
            }

            _context.Account.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
                throw new KeyNotFoundException("Invalid GUID format.");

            var account = await _context.Account.FindAsync(guidId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found.");
            }

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
        }
    }