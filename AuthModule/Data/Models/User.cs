


using System.Reflection.Metadata.Ecma335;
using AuthModule.Data.Models.Abstract;

namespace AuthModule.Data.Models
{
    public class User : IUser<User, int>
    {
        public int Id { get; set; }
        public string Handle { get; set; }
        public string Password { get; set; }
        public List<Claim<User>> Claims { get; set; } = new();
        public List<Role<User>> Roles { get ; set ; } = new();
    }
}
