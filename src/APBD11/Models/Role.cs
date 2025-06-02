namespace APBD11.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    public const string Admin = "Admin";
    public const string User = "User";
}