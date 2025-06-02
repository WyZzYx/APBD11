using System.ComponentModel.DataAnnotations;

namespace APBD11.DTOs;

public class AccountRegisterDto
{
    [Required]
    [RegularExpression(@"^[A-Za-z][A-Za-z0-9]*$",
        ErrorMessage = "Username must start with a letter and contain only letters and digits.")]
    public string Username { get; set; }

    [Required]
    [MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$",
        ErrorMessage = "Password must contain at least one lowercase, one uppercase, one digit, and one symbol.")]
    public string Password { get; set; }

    [Required]
    [Range(1, 2, ErrorMessage = "RoleId must be 1 (Admin) or 2 (User).")]
    public int RoleId { get; set; }
}