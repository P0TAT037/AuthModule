namespace AuthModule.Data.Models
{
    public interface IUser<TUser, TId>
    {
        public TId Id { get; set; }

        public string Handle { get; set; }

        public string Password { get; set; }

        public IEnumerable<Claim<TUser>> Claims { get; set;}
        public IEnumerable<Role<TUser>> Roles { get; set;}
    }
}
