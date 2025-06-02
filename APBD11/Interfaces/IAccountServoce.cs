using APBD11.DTOs;
using APBD11.Models;
using DTOs;

namespace APBD11.Interfaces;

public interface IAccountService
{
    Task<Account> RegisterAsync(AccountRegisterDto registerDto);
    Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto authDto);
    Task<Account> GetByIdAsync(string id);
    Task<IEnumerable<Account>> GetAllAsync();
    Task UpdateAsync(string id, AccountRegisterDto updateDto, string currentUserId, bool isAdmin);
    Task DeleteAsync(string id);
}