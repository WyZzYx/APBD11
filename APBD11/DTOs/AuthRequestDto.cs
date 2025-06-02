using System.ComponentModel.DataAnnotations;

namespace APBD11.DTOs;

public class AuthRequestDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}