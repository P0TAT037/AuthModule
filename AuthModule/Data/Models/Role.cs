using System.ComponentModel.DataAnnotations.Schema;

namespace AuthModule.Data.Models
{
    public class Role<TUser>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Claim<TUser>> Claims { get; set; } = new();

        public List<TUser> Users { get; set; } = new();
    }
}
