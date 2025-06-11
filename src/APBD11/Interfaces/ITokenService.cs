namespace APBD11.Interfaces;

public interface ITokenService
{
    string GenerateToken(string username, string role);
}