using AuthModule.Data.Models;
using AuthModule.Data.Models.Abstract;

namespace Sample.Models;

public class User : IUser<User, int>
{
    public int Id { get; set; }
    public string Handle { get; set; }
    public string Password { get; set; }
    public List<Claim<User>> Claims { get; set; } = new();
    public List<Role<User>> Roles { get ; set ; } = new();
}
