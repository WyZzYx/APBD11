using System.ComponentModel.DataAnnotations;

namespace APBD11.Models;

public class Position
{
    public int Id { get; set; }

    [Required, Length(1, 100)] public string Name { get; set; } = null!;
    public int MinExpYears { get; set; }
}