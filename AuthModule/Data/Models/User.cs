

namespace AuthModule.Data.Models
{
    public class User : IUser<int>
    {
        public int Id { get; set; }
        public string Handle { get; set; }
        public string Password { get; set; }
        public List<Claim<IUser<int>>> Claims { get; set; } = new();
        public List<Role<IUser<int>>> Roles { get; set; } = new();
    }
}
