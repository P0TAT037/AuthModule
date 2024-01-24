using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace AuthModule.Data.Models
{
    public class Role<TUser>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Claim<TUser>> Claims { get; set; } = new();

        public List<TUser> Users { get; set; } = new();

        public Claim<TUser> GetClaim()
        {
            return new Claim<TUser>() { Name = ClaimTypes.Role, Value = Name};
        }
    }
}
