using System.ComponentModel.DataAnnotations;

namespace APBD11.Models;

public class Person
{
    public int Id { get; set; }

    [Length(1, 30)] [Required] public string PassportNumber { get; set; } = null!;

    [Length(1, 60)]
    [Required]
    public string FirstName { get; set; } = null!;
    [StringLength(100)]
    public string? MiddleName { get; set; }
    [Length(1, 60)]
    [Required]
    public string LastName { get; set; } = null!;
    [Length(1, 60)]
    [Required]
    public string PhoneNumber { get; set; } = null!;
    [Length(1, 60)]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}