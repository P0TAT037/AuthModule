namespace AuthModule.Data.Models.Abstract
{
    public interface IUser<TUser, TId>
    {
        public TId Id { get; set; }

        public string Handle { get; set; }

        public string Password { get; set; }

        public List<Claim<TUser>> Claims { get; set; }
        public List<Role<TUser>> Roles { get; set; }
    }
}
