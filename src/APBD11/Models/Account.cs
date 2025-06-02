using System.ComponentModel.DataAnnotations;

namespace APBD11.Models;

public class Account
{
    
    public Guid Id { get; set; }
    [Length(1, 50)]
    public string Username { get; set; }
    [Length(1, 50)]
    public string Password { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; }

    public ICollection<Device> Devices { get; set; }

    public Account()
    {
        Devices = new HashSet<Device>();
    }
}