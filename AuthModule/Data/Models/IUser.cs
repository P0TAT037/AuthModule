namespace AuthModule.Data.Models
{
    public interface IUser<TId>
    {
        public TId Id { get; set; }

        public string Handle { get; set; }

        public string Password { get; set; }

        public List<Claim<IUser<TId>>> Claims { get; set; }

        public List<Role<IUser<TId>>> Roles { get; set; }
    }
}
