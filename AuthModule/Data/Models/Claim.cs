using System.ComponentModel.DataAnnotations.Schema;

namespace AuthModule.Data.Models
{
    public class Claim<TUser>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public List<TUser> Users { get; set; } = new();

        public List<Role<TUser>> Roles { get; set; } = new();
    }
}
