


using System.Reflection.Metadata.Ecma335;

namespace AuthModule.Data.Models
{
    public class User : IUser<User, int>
    {
        public int Id { get; set; }
        public string Handle { get; set; }
        public string Password { get; set; }
        public IEnumerable<Claim<User>> Claims { get; set; } = Enumerable.Empty<Claim<User>>();
        public IEnumerable<Role<User>> Roles { get ; set ; } = Enumerable.Empty<Role<User>>();
    }
}
